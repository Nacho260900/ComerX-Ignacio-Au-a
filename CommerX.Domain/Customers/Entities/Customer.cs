using System;
using CommerX.Domain.Common.Entities;
using CommerX.Domain.Customers.ValueObjects;

namespace CommerX.Domain.Customers.Entities;

public sealed class Customer : BaseEntity
{
    public FullName FullName { get; private set; } = null!;
    public LastName LastName { get; private set; } = null!;
    public Document Document { get; private set; } = null!;
    public EmailAddress Email { get; private set; } = null!;
    public Phone Phone { get; private set; } = null!;
    public Address Address { get; private set; } = null!;
    public BirthDate BirthDate { get; private set; } = null!;

    private Customer() { }

    public static Customer Create(
        string fullName,
        string lastName,
        string document,
        string email,
        string phone,
        string address,
        DateOnly birthDate)
    {
        return new Customer
        {
            FullName = FullName.Create(fullName),
            LastName = LastName.Create(lastName),
            Document = Document.Create(document),
            Email = EmailAddress.Create(email),
            Phone = Phone.Create(phone),
            Address = Address.Create(address),
            BirthDate = BirthDate.Create(birthDate)
        };
    }

    public void UpdateEmail(string newEmail)
        => Email = EmailAddress.Create(newEmail);

    public void UpdateAddress(string newAddress)
        => Address = Address.Create(newAddress);
}