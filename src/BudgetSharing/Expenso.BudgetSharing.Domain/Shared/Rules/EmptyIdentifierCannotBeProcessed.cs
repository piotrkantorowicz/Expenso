using System.Reflection;

using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Rules;

internal sealed class EmptyIdentifierCannotBeProcessed<T> : IBusinessRule
{
    private readonly T? _identifier;
    private readonly MemberInfo? _type;

    public EmptyIdentifierCannotBeProcessed(T? identifier, MemberInfo? type = null)
    {
        _identifier = identifier;
        _type = type;
    }

    public string Message => $"Empty identifier {_type?.Name} cannot be processed.";

    public bool IsBroken()
    {
        if (_identifier is null)
        {
            return true;
        }

        return _identifier switch
        {
            Guid guidIdentifier => guidIdentifier == Guid.Empty,
            string stringIdentifier => string.IsNullOrWhiteSpace(value: stringIdentifier),
            int intIdentifier => intIdentifier == 0,
            _ => throw new InvalidOperationException(message: $"Unsupported identifier type: {typeof(T)}")
        };
    }
}