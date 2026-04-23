using System;
using CommerX.Domain.Common.Exceptions;

namespace CommerX.Domain.Customers.Exceptions;

public sealed class InvalidAgeException : DomainException
{
    public InvalidAgeException(DateOnly birthDate)
        : base($"La fecha de nacimiento '{birthDate:dd/MM/yyyy}' no cumple el requisito de mayoría de edad (18 años).") { }
}
