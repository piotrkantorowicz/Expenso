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
        [
            new BusinesRuleCheck(
                BusinessRule: new EmptyDateTimeCannotBeProcessed(dateTimeOffset: value, type: typeof(DateAndTime)))
        ]);

        return new DateAndTime(value: value);
    }

    public static DateAndTime? Nullable(DateTimeOffset? value)
    {
        return value is null || value == DateTimeOffset.MinValue || value == DateTimeOffset.MaxValue
            ? null
            : new DateAndTime(value: value.Value);
    }

    public static implicit operator DateAndTime(DateTimeOffset dateTimeOffset)
    {
        return New(value: dateTimeOffset);
    }

    public static implicit operator DateTimeOffset(DateAndTime dateAndTime)
    {
        return dateAndTime.Value;
    }

    public bool GreaterThan(DateAndTime dateTimeOffset)
    {
        return Value > dateTimeOffset.Value;
    }

    public bool LessThan(DateAndTime dateTimeOffset)
    {
        return Value < dateTimeOffset.Value;
    }

    public bool GreaterThanOrEqual(DateAndTime dateTimeOffset)
    {
        return Value >= dateTimeOffset.Value;
    }

    public bool LessThanOrEqual(DateAndTime dateTimeOffset)
    {
        return Value <= dateTimeOffset.Value;
    }

    public bool Between(DateAndTime start, DateAndTime end)
    {
        return Value >= start.Value && Value <= end.Value;
    }

    public bool InRange(DateAndTime start, DateAndTime end)
    {
        return Value >= start.Value && Value <= end.Value;
    }

    public bool OutOfRange(DateAndTime start, DateAndTime end)
    {
        return Value < start.Value || Value > end.Value;
    }
}