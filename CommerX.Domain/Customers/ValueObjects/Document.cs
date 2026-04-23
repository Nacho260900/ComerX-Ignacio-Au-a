using System.Text.RegularExpressions;
using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record Document : ValueObject
{
    public string Value { get; }
    private Document(string value) => Value = value;

    public static Document Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidDocumentException(value ?? string.Empty);

        var trimmed = value.Trim();

        if (!Regex.IsMatch(trimmed, @"^\d{7,15}$"))
            throw new InvalidDocumentException(trimmed);

        return new Document(trimmed);
    }
}
