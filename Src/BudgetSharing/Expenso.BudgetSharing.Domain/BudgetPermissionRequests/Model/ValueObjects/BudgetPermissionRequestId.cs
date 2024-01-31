using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;

public sealed class BudgetPermissionRequestId
{
    private readonly DomainModelState _domainModelState = new();

    private BudgetPermissionRequestId() : this(Guid.Empty)
    {
        Value = Guid.Empty;
    }

    private BudgetPermissionRequestId(Guid value)
    {
        _domainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(BudgetPermissionRequestId budgetPermissionRequestId)
    {
        return budgetPermissionRequestId.Value;
    }

    public static implicit operator BudgetPermissionRequestId(Guid id)
    {
        return new BudgetPermissionRequestId(id);
    }

    internal static BudgetPermissionRequestId CreateDefault()
    {
        return new BudgetPermissionRequestId();
    }

    public static BudgetPermissionRequestId Create(Guid value)
    {
        return new BudgetPermissionRequestId(value);
    }
}