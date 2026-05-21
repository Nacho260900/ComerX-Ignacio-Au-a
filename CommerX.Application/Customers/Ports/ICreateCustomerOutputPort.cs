using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;

namespace CommerX.Application.Customers.Ports
{
    public interface ICreateCustomerOutputPort
    {
        //el cliente fue creado exitosamente
        Task HandleSuccessAsync(CreateCustomerResponse response);
        //ya existe un cliente con el mismo documento - regla de unicidad violada
        Task HandleDuplicateAsync(string document);
        // una regla de dominio fue violada - mensaje descriptivo
        Task HandleValidationErrorAsync(string message);
    }
    

}
