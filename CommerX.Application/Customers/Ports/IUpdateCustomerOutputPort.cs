using System.Collections.Generic;
using System.Threading.Tasks;
using CommerX.Application.Customers.DTOs;

namespace CommerX.Application.Customers.Ports
{
    //define la interfaz de salida del caso de uso UpdateCustomer
    public interface IUpdateCustomerOutputPort
    {
        //propiedad para obtener el resultado de la operacion
        UpdateCustomerResponse Response { get; }
        
        //metodo que se invoca cuando la operacion fue exitosa
        Task HandleSuccessAsync(UpdateCustomerResponse response);

        // metodo que se invoca cuando el correo ya existe
        Task HandleDuplicateAsync(string email);

        //metodo que se invoca cuando una regla de dominio fue violada - mensaje descriptivo
        Task HandleValidationErrorAsync(string message);

        //metodo que se invoca cuando hay varios errores en la validacion del DTO
        Task ValidationErrorsAsync(IReadOnlyList<string> errors);
    }
}
