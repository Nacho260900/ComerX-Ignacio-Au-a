using System;
using CommerX.Domain.Common.ValueObjects;
using CommerX.Domain.Customers.Exceptions;

namespace CommerX.Domain.Customers.ValueObjects;

public sealed record BirthDate : ValueObject
{
    private const int MinimumAge = 18;

    public DateOnly Value { get; }
    private BirthDate(DateOnly value) => Value = value;

    public static BirthDate Create(DateOnly value)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        if (value > today)
            throw new InvalidAgeException(value);

        var age = today.Year - value.Year;
        if (value.AddYears(age) > today) age--;

        if (age < MinimumAge)
            throw new InvalidAgeException(value);

        return new BirthDate(value);
    }
}
