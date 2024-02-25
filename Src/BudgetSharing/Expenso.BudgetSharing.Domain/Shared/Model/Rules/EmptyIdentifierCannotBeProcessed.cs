using System.Reflection;

using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.Rules;

internal sealed class EmptyIdentifierCannotBeProcessed(Guid? identifier, MemberInfo? type = null) : IBusinessRule
{
    public string Message => $"Empty identifier {type?.Name} cannot be processed";

    public bool IsBroken()
    {
        return identifier is null || identifier == Guid.Empty;
    }
}