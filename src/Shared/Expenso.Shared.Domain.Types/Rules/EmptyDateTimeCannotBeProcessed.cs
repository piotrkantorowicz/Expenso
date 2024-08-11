using System.Reflection;

namespace Expenso.Shared.Domain.Types.Rules;

internal sealed class EmptyDateTimeCannotBeProcessed(DateTimeOffset? dateTimeOffset, MemberInfo? type = null)
    : IBusinessRule
{
    public string Message => $"Empty date and time {type?.Name} cannot be processed";

    public bool IsBroken()
    {
        return dateTimeOffset is null || dateTimeOffset == DateTimeOffset.MinValue ||
               dateTimeOffset == DateTimeOffset.MaxValue;
    }
}