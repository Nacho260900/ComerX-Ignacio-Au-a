using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;
using CommerX.Application.Customers.Ports;
using CommerX.Domain.Common.Exceptions;
using CommerX.Domain.Customers.Entities;
using CommerX.Domain.Customers.Repositories;
using CommerX.Domain.Customers.ValueObjects;

namespace CommerX.Application.Customers.UseCases
{
    public sealed class UpdateCustomerUseCase : IUpdateCustomerInputPort
    {
        private readonly ICustomerRepository _repository;
        private readonly IUpdateCustomerOutputPort _outputPort;

        public UpdateCustomerUseCase(
            ICustomerRepository repository,
            IUpdateCustomerOutputPort outputPort)
        {
            _repository = repository;
            _outputPort = outputPort;
        }

        public async Task ExecuteAsync(UpdateCustomerRequest request)
        {
            if (request == null)
            {
                await _outputPort.HandleValidationErrorAsync("La petición de actualización no puede ser nula.");
                return;
            }

            // 1. Validar formatos de entrada (Value Objects) acumulando errores
            var errors = new List<string>();

            try
            {
                EmailAddress.Create(request.Email);
            }
            catch (DomainException ex)
            {
                errors.Add(ex.Message);
            }

            try
            {
                Phone.Create(request.Phone);
            }
            catch (DomainException ex)
            {
                errors.Add(ex.Message);
            }

            try
            {
                Address.Create(request.Address);
            }
            catch (DomainException ex)
            {
                errors.Add(ex.Message);
            }

            try
            {
                BirthDate.Create(request.BirthDate);
            }
            catch (DomainException ex)
            {
                errors.Add(ex.Message);
            }

            if (errors.Any())
            {
                await _outputPort.ValidationErrorsAsync(errors);
                return;
            }

            try
            {
                // 2. Buscar si el cliente existe
                var customer = await _repository.FindByIdAsync(request.CustomerId);
                if (customer == null)
                {
                    await _outputPort.HandleValidationErrorAsync($"El cliente con ID '{request.CustomerId}' no existe en el sistema.");
                    return;
                }

                // 3. Validar unicidad del email (excluyendo al cliente actual)
                var emailExists = await _repository.ExistsByEmailExcludeAsync(request.Email, request.CustomerId);
                if (emailExists)
                {
                    await _outputPort.HandleDuplicateAsync(request.Email);
                    return;
                }

                // 4. Validar existencia de cambios
                var normalizedEmail = request.Email.Trim().ToLowerInvariant();
                var trimmedPhone = request.Phone.Trim();
                var trimmedAddress = request.Address.Trim();

                if (customer.Email.Value == normalizedEmail &&
                    customer.Phone.Value == trimmedPhone &&
                    customer.Address.Value == trimmedAddress &&
                    customer.BirthDate.Value == request.BirthDate)
                {
                    await _outputPort.HandleValidationErrorAsync("No se detectaron cambios en los datos del cliente para actualizar.");
                    return;
                }

                // 5. Aplicar la actualización en el dominio
                customer.Update(request.Email, request.Phone, request.Address, request.BirthDate);

                // 6. Guardar cambios en el repositorio
                await _repository.UpdateAsync(customer);

                // 7. Notificar éxito
                var updatedData = new UpdatedCustomerData(
                    customer.Email.Value,
                    customer.Phone.Value,
                    customer.Address.Value,
                    customer.BirthDate.Value
                );
                var response = new UpdateCustomerResponse(customer.Id, updatedData);

                await _outputPort.HandleSuccessAsync(response);
            }
            catch (DomainException ex)
            {
                await _outputPort.HandleValidationErrorAsync(ex.Message);
            }
        }
    }
}
