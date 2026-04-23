using CommerX.Domain.Common.Exceptions;

namespace CommerX.Domain.Customers.Exceptions;

public sealed class InvalidFullNameException : DomainException
{
    public InvalidFullNameException(string value)
        : base($"El nombre '{value}' no es válido. Debe tener entre 2 y 100 caracteres.") { }
}
