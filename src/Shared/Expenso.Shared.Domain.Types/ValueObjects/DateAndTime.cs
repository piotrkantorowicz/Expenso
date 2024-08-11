using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.Shared.Domain.Types.ValueObjects;

public sealed record DateAndTime
{
    private DateAndTime(DateTimeOffset value)
    {
        Value = value;
    }

    public DateTimeOffset Value { get; }

    public static DateAndTime New(DateTimeOffset value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
            [(new EmptyDateTimeCannotBeProcessed(dateTimeOffset: value, type: typeof(DateAndTime)), false)]);

        return new DateAndTime(value: value);
    }

    public static DateAndTime? Nullable(DateTimeOffset? value)
    {
        return value is null || value == DateTimeOffset.MinValue || value == DateTimeOffset.MaxValue
            ? null
            : new DateAndTime(value: value.Value);
    }

    public static bool operator >(DateTimeOffset left, DateAndTime right)
    {
        return left > right.Value;
    }

    public static bool operator <(DateTimeOffset left, DateAndTime right)
    {
        return left < right.Value;
    }

    public static bool operator >=(DateTimeOffset left, DateAndTime right)
    {
        return left >= right.Value;
    }

    public static bool operator <=(DateTimeOffset left, DateAndTime right)
    {
        return left <= right.Value;
    }

    public static bool operator >(DateAndTime left, DateTimeOffset right)
    {
        return left.Value > right;
    }

    public static bool operator <(DateAndTime left, DateTimeOffset right)
    {
        return left.Value < right;
    }

    public static bool operator >=(DateAndTime left, DateTimeOffset right)
    {
        return left.Value >= right;
    }

    public static bool operator <=(DateAndTime left, DateTimeOffset right)
    {
        return left.Value <= right;
    }

    public bool GreaterThan(DateTimeOffset dateTimeOffset)
    {
        return Value > dateTimeOffset;
    }

    public bool LessThan(DateTimeOffset dateTimeOffset)
    {
        return Value < dateTimeOffset;
    }

    public bool GreaterThanOrEqual(DateTimeOffset dateTimeOffset)
    {
        return Value >= dateTimeOffset;
    }

    public bool LessThanOrEqual(DateTimeOffset dateTimeOffset)
    {
        return Value <= dateTimeOffset;
    }

    public bool Between(DateTimeOffset start, DateTimeOffset end)
    {
        return Value >= start && Value <= end;
    }

    public bool InRange(DateTimeOffset start, DateTimeOffset end)
    {
        return Value >= start && Value <= end;
    }

    public bool OutOfRange(DateTimeOffset start, DateTimeOffset end)
    {
        return Value < start || Value > end;
    }
}