using System;
namespace CommerX.Application.Customers.DTOs
{
    public sealed class UpdateCustomerRequest
    {
        public Guid CustomerId { get; init; }
        public string Email { get; init; }
        public string Phone { get; init; }
        public string Address { get; init; }
        public DateOnly BirthDate { get; init; }
        // Constructor explícito
        public UpdateCustomerRequest(Guid customerId, string email, string phone, string address, DateOnly birthDate)
        {
            CustomerId = customerId;
            Email = email;
            Phone = phone;
            Address = address;
            BirthDate = birthDate;
        }
    }
}