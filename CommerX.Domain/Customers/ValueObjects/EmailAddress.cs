using System.Text.RegularExpressions;
using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record EmailAddress : ValueObject
{
    private static readonly Regex EmailRegex = 
        new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled);

    public string Value { get; }
    private EmailAddress(string value) => Value = value;

    public static EmailAddress Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new InvalidEmailException(value ?? string.Empty);

        var normalized = value.Trim().ToLowerInvariant();

        if (!EmailRegex.IsMatch(normalized))
            throw new InvalidEmailException(normalized);

        return new EmailAddress(normalized);
    }
}
