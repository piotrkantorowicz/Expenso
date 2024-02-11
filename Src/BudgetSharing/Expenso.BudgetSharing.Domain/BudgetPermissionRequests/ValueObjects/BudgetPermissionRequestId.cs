using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

public sealed class BudgetPermissionRequestId
{
    private BudgetPermissionRequestId(Guid value)
    {
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
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

    public static BudgetPermissionRequestId Create(Guid value)
    {
        return new BudgetPermissionRequestId(value);
    }
}