using System;
using CommerX.Domain.Common.Exceptions;
using CommerX.Domain.Customers.Entities;
using CommerX.Domain.Customers.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CustomerTest
{
    class Program
    {
        static void Main(string[] args)
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
        }
    }
}
