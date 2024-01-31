using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

public sealed class BudgetPermissionId
{
    private readonly DomainModelState _domainModelState = new();

    private BudgetPermissionId()
    {
        Value = Guid.Empty;
    }

    private BudgetPermissionId(Guid value)
    {
        _domainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(BudgetPermissionId id)
    {
        return id.Value;
    }

    public static implicit operator BudgetPermissionId(Guid id)
    {
        return new BudgetPermissionId(id);
    }

    internal static BudgetPermissionId CreateDefault()
    {
        return new BudgetPermissionId();
    }

    public static BudgetPermissionId Create(Guid value)
    {
        return new BudgetPermissionId(value);
    }
}