using CommerX.Domain.Common.Exceptions;

namespace CommerX.Domain.Customers.Exceptions;

public sealed class InvalidEmailException : DomainException
{
    public InvalidEmailException(string value)
        : base($"El email '{value}' no tiene formato válido (ej: usuario@dominio.com).") { }
}
