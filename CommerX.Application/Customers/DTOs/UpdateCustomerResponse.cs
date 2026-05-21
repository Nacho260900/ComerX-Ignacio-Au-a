using System;

namespace CommerX.Application.Customers.DTOs
{
    // Clase de apoyo para representar los datos que fueron modificados
    public class UpdatedCustomerData
    {
        public string Email { get; init; } = string.Empty;
        public string Phone { get; init; } = string.Empty;
        public string Address { get; init; } = string.Empty;
        public DateOnly BirthDate { get; init; }

        public UpdatedCustomerData(string email, string phone, string address, DateOnly birthDate)
        {
            Email = email;
            Phone = phone;
            Address = address;
            BirthDate = birthDate;
        }
    }

    // clase de respuesta sellada (sealed)
    public sealed class UpdateCustomerResponse
    {
        public Guid CustomerID { get; init; }

        // propiedad requerida de tipo init para los datos modificados
        public UpdatedCustomerData Data { get; init; } = null!;

        // constructor que recibe los valores
        public UpdateCustomerResponse(Guid customerId, UpdatedCustomerData updateData)
        {
            CustomerID = customerId;
            Data = updateData;
        }
    }
}
