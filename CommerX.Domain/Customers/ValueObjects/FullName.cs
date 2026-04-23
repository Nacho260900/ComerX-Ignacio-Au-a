using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record FullName : ValueObject
{
    public string Value { get; }

    private FullName(string value) => Value = value;

    public static FullName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidFullNameException(value ?? string.Empty);

        var trimmed = value.Trim();

        if (trimmed.Length < 2 || trimmed.Length > 100)
            throw new InvalidFullNameException(trimmed);

        return new FullName(trimmed);
    }
}
