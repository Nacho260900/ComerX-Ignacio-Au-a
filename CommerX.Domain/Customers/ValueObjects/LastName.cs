using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record LastName : ValueObject
{
    public string Value { get; }
    private LastName(string value) => Value = value;

    public static LastName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidLastNameException(value ?? string.Empty);

        var trimmed = value.Trim();

        if (trimmed.Length < 2 || trimmed.Length > 100)
            throw new InvalidLastNameException(trimmed);

        return new LastName(trimmed);
    }
}
