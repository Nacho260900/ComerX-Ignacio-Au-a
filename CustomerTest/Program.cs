using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;
using CommerX.Application.Customers.Ports;
using CommerX.Application.Customers.UseCases;
using CommerX.Domain.Common.Exceptions;
using CommerX.Domain.Customers.Entities;
using CommerX.Domain.Customers.Exceptions;
using CommerX.Domain.Customers.Repositories;
using CommerX.Domain.Customers.ValueObjects;

namespace CustomerTest
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("--- Verificando Excepciones de Dominio (Fail Fast) ---");
            
            try
            {
                // Prueba 1: Email inválido
                Console.WriteLine("\nIntentando crear email inválido...");
                EmailAddress.Create("correo-sin-arroba");
            }
            catch (InvalidEmailException ex)
            {
                Console.WriteLine($"Capturada InvalidEmailException: {ex.Message}");
            }

            try
            {
                // Prueba 2: Edad inválida
                Console.WriteLine("\nIntentando crear fecha de nacimiento para menor de edad...");
                BirthDate.Create(DateOnly.FromDateTime(DateTime.Today.AddYears(-10)));
            }
            catch (InvalidAgeException ex)
            {
                Console.WriteLine($"Capturada InvalidAgeException: {ex.Message}");
            }

            try
            {
                // Prueba 3: Captura genérica con DomainException
                Console.WriteLine("\nIntentando crear documento inválido (captura genérica)...");
                Document.Create("123"); // Muy corto
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"Capturada DomainException: {ex.Message}");
            }

            try
            {
                // Prueba 4: Flujo exitoso
                Console.WriteLine("\nCreando Customer válido...");
                var customer = Customer.Create(
                    "Juan",
                    "Pérez",
                    "12345678",
                    "juan@mail.com",
                    "2611234567",
                    "Calle Falsa 123, Mendoza",
                    new DateOnly(1990, 1, 1));

                Console.WriteLine("Customer creado con éxito.");
                Console.WriteLine($"Email: {customer.Email.Value}");
            }
            catch (DomainException ex)
            {
                Console.WriteLine($"ERROR INESPERADO: {ex.Message}");
            }

            Console.WriteLine("\n=======================================================");
            Console.WriteLine("--- Verificando Caso de Uso: Actualizar Cliente (CU-CLI-002) ---");
            Console.WriteLine("=======================================================\n");

            // Setup
            var repository = new FakeCustomerRepository();
            var outputPort = new FakeUpdateCustomerOutputPort();
            var useCase = new UpdateCustomerUseCase(repository, outputPort);

            // Crear y guardar clientes iniciales
            var customer1 = Customer.Create("Juan", "Pérez", "12345678", "juan@mail.com", "2611234567", "Calle Falsa 123", new DateOnly(1990, 1, 1));
            var customer2 = Customer.Create("María", "López", "87654321", "maria@mail.com", "2617654321", "Calle Verdadera 456", new DateOnly(1992, 2, 2));
            await repository.AddAsync(customer1);
            await repository.AddAsync(customer2);

            // PRUEBA 1: Flujo Exitoso (Success Path)
            Console.WriteLine("[Prueba 1] Modificar todos los campos válidamente...");
            var successRequest = new UpdateCustomerRequest(
                customer1.Id,
                "juan.nuevo@mail.com",
                "2619999999",
                "Calle Nueva 789",
                new DateOnly(1991, 11, 11)
            );
            await useCase.ExecuteAsync(successRequest);
            Console.WriteLine($"Resultado de llamada: {outputPort.LastCall}");
            if (outputPort.LastCall == "Success")
            {
                Console.WriteLine($"  ✔ ID del cliente modificado: {outputPort.Response!.CustomerID}");
                Console.WriteLine($"  ✔ Email nuevo: {outputPort.Response.Data.Email}");
                Console.WriteLine($"  ✔ Teléfono nuevo: {outputPort.Response.Data.Phone}");
                Console.WriteLine($"  ✔ Dirección nueva: {outputPort.Response.Data.Address}");
                Console.WriteLine($"  ✔ Fecha nac. nueva: {outputPort.Response.Data.BirthDate}");
            }
            else
            {
                Console.WriteLine("  ❌ FALLÓ PRUEBA 1");
            }

            // PRUEBA 2: Errores de Validación Múltiples (ValidationErrorsAsync)
            Console.WriteLine("\n[Prueba 2] Intentando actualizar con email y teléfono inválidos al mismo tiempo...");
            outputPort.Reset();
            var invalidRequest = new UpdateCustomerRequest(
                customer1.Id,
                "formato-email-incorrecto",
                "12", // Teléfono muy corto
                "Calle Nueva 789",
                new DateOnly(1991, 11, 11)
            );
            await useCase.ExecuteAsync(invalidRequest);
            Console.WriteLine($"Resultado de llamada: {outputPort.LastCall}");
            if (outputPort.LastCall == "ValidationErrors")
            {
                Console.WriteLine("  ✔ Errores recibidos:");
                foreach (var err in outputPort.ValidationErrors!)
                {
                    Console.WriteLine($"    - {err}");
                }
            }
            else
            {
                Console.WriteLine("  ❌ FALLÓ PRUEBA 2");
            }

            // PRUEBA 3: Cliente no encontrado (HandleValidationErrorAsync)
            Console.WriteLine("\n[Prueba 3] Intentando actualizar un cliente que no existe...");
            outputPort.Reset();
            var nonExistentRequest = new UpdateCustomerRequest(
                Guid.NewGuid(),
                "juan.nuevo@mail.com",
                "2619999999",
                "Calle Nueva 789",
                new DateOnly(1991, 11, 11)
            );
            await useCase.ExecuteAsync(nonExistentRequest);
            Console.WriteLine($"Resultado de llamada: {outputPort.LastCall}");
            Console.WriteLine($"  ✔ Mensaje: {outputPort.ValidationError}");

            // PRUEBA 4: Correo electrónico duplicado (HandleDuplicateAsync)
            Console.WriteLine("\n[Prueba 4] Intentando asignar un email que ya posee otro cliente...");
            outputPort.Reset();
            var duplicateRequest = new UpdateCustomerRequest(
                customer1.Id,
                "maria@mail.com", // Pertenece a customer2
                "2619999999",
                "Calle Nueva 789",
                new DateOnly(1991, 11, 11)
            );
            await useCase.ExecuteAsync(duplicateRequest);
            Console.WriteLine($"Resultado de llamada: {outputPort.LastCall}");
            Console.WriteLine($"  ✔ Correo en conflicto: {outputPort.DuplicateEmail}");

            // PRUEBA 5: Sin cambios detectados (HandleValidationErrorAsync)
            Console.WriteLine("\n[Prueba 5] Intentando guardar sin realizar ningún cambio...");
            outputPort.Reset();
            // Los datos actuales de customer2 son los originales
            var noChangesRequest = new UpdateCustomerRequest(
                customer2.Id,
                "maria@mail.com",
                "2617654321",
                "Calle Verdadera 456",
                new DateOnly(1992, 2, 2)
            );
            await useCase.ExecuteAsync(noChangesRequest);
            Console.WriteLine($"Resultado de llamada: {outputPort.LastCall}");
            Console.WriteLine($"  ✔ Mensaje: {outputPort.ValidationError}");
        }
    }

    // --- Clases Auxiliares de Prueba (Fakes) ---

    public class FakeCustomerRepository : ICustomerRepository
    {
        private readonly List<Customer> _customers = new();

        public Task AddAsync(Customer customer)
        {
            _customers.Add(customer);
            return Task.CompletedTask;
        }

        public Task<Customer?> FindByDocumentAsync(string document)
        {
            var customer = _customers.FirstOrDefault(c => c.Document.Value == document);
            return Task.FromResult(customer);
        }

        public Task<Customer?> FindByIdAsync(Guid id)
        {
            var customer = _customers.FirstOrDefault(c => c.Id == id);
            return Task.FromResult(customer);
        }

        public Task<bool> ExistsByEmailExcludeAsync(string email, Guid currentCustomerId)
        {
            var normalizedEmail = email.Trim().ToLowerInvariant();
            var exists = _customers.Any(c => c.Id != currentCustomerId && c.Email.Value == normalizedEmail);
            return Task.FromResult(exists);
        }

        public Task UpdateAsync(Customer customer)
        {
            var index = _customers.FindIndex(c => c.Id == customer.Id);
            if (index != -1)
            {
                _customers[index] = customer;
            }
            return Task.CompletedTask;
        }
    }

    public class FakeUpdateCustomerOutputPort : IUpdateCustomerOutputPort
    {
        public UpdateCustomerResponse Response { get; private set; } = null!;
        public string? DuplicateEmail { get; private set; }
        public string? ValidationError { get; private set; }
        public IReadOnlyList<string>? ValidationErrors { get; private set; }
        public string LastCall { get; private set; } = string.Empty;

        public Task HandleSuccessAsync(UpdateCustomerResponse response)
        {
            Response = response;
            LastCall = "Success";
            return Task.CompletedTask;
        }

        public Task HandleDuplicateAsync(string email)
        {
            DuplicateEmail = email;
            LastCall = "Duplicate";
            return Task.CompletedTask;
        }

        public Task HandleValidationErrorAsync(string message)
        {
            ValidationError = message;
            LastCall = "ValidationError";
            return Task.CompletedTask;
        }

        public Task ValidationErrorsAsync(IReadOnlyList<string> errors)
        {
            ValidationErrors = errors;
            LastCall = "ValidationErrors";
            return Task.CompletedTask;
        }

        public void Reset()
        {
            Response = null!;
            DuplicateEmail = null;
            ValidationError = null;
            ValidationErrors = null;
            LastCall = string.Empty;
        }
    }
}
