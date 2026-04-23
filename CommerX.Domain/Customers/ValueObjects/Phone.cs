using System.Text.RegularExpressions;
using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record Phone : ValueObject
{
    public string Value { get; }
    private Phone(string value) => Value = value;

    public static Phone Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidPhoneException(value ?? string.Empty);

        var trimmed = value.Trim();

        if (!Regex.IsMatch(trimmed, @"^\d{7,15}$"))
            throw new InvalidPhoneException(trimmed);

        return new Phone(trimmed);
    }
}
