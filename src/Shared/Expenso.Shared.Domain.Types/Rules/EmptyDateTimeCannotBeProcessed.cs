using System.Reflection;

namespace Expenso.Shared.Domain.Types.Rules;

internal sealed class EmptyDateTimeCannotBeProcessed : IBusinessRule
{
    private readonly DateTimeOffset? _dateTimeOffset;
    private readonly MemberInfo? _type;

    public EmptyDateTimeCannotBeProcessed(DateTimeOffset? dateTimeOffset, MemberInfo? type = null)
    {
        _dateTimeOffset = dateTimeOffset;
        _type = type;
    }

    public string Message => $"Empty date and time {_type?.Name} cannot be processed.";

    public bool IsBroken()
    {
        return _dateTimeOffset is null || _dateTimeOffset == DateTimeOffset.MinValue ||
               _dateTimeOffset == DateTimeOffset.MaxValue;
    }
}