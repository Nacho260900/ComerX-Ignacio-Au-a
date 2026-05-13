using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;
using CommerX.Application.Customers.Ports;
using CommerX.Domain.Common.Exceptions;
using CommerX.Domain.Customers.Entities;
using CommerX.Domain.Customers.Repositories;

namespace CommerX.Application.Customers.UseCases
{
    public sealed class CreateCustomerUseCase : ICreateCustomerInputPort
    {
        //repositorio inyectado - contrato definido en Domain
        private readonly ICustomerRepository _repository;
        // puerto de salida inyectado - notifica el resultado al llamador
        private readonly ICreateCustomerOutputPort _outputPort;

        public CreateCustomerUseCase(
            ICustomerRepository repository,
            ICreateCustomerOutputPort outputPort)
        {
            _repository = repository;
            _outputPort = outputPort;
        }

        public async Task ExecuteAsync(CreateCustomerRequest request)
        {
            try
            {                 //verificar si ya existe un cliente con el mismo documento
                var existingCustomer = await _repository.FindByDocumentAsync(request.Document);
                if (existingCustomer != null)
                {
                    // notificamos la duplicidad y detenemos la ejcucion
                    await _outputPort.HandleDuplicateAsync(request.Document);
                    return;
                }
                //el dominio valida las reglas - puede lanzar DomainException
                var customer = Customer.Create(
                    request.FirstName,
                    request.LastName,
                    request.Document,
                    request.Email,
                    request.Phone,
                    request.Address,
                    request.BirthDate
                );

                //persistir el nuevo cliente en el repositorio
                await _repository.AddAsync(customer);

                // construimos el Dto de respuesta y notificamos el éxito al llamador
                var response = new CreateCustomerResponse
                {
                    CustomerID = customer.Id,
                };

                await _outputPort.HandleSuccessAsync(response);
            }
            catch (DomainException ex)
            {
                // cualquier excepcion de dominio se redirige al puerto de salida como error de validación
                await _outputPort.HandleValidationErrorAsync(ex.Message);

            }
        }
    }
}
