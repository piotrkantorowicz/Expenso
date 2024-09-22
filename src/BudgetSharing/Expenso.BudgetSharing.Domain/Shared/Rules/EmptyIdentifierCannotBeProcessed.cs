using System.Reflection;

using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Rules;

internal sealed class EmptyIdentifierCannotBeProcessed : IBusinessRule
{
    private readonly Guid? _identifier;
    private readonly MemberInfo? _type;

    public EmptyIdentifierCannotBeProcessed(Guid? identifier, MemberInfo? type = null)
    {
        _identifier = identifier;
        _type = type;
    }

    public string Message => $"Empty identifier {_type?.Name} cannot be processed.";

    public bool IsBroken()
    {
        return _identifier is null || _identifier == Guid.Empty;
    }
}