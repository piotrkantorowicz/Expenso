using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

public sealed record BudgetPermissionRequestId
{
    private BudgetPermissionRequestId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetPermissionRequestId New(Guid value)
    {
        DomainModelState.CheckBusinessRules([
            new EmptyIdentifierCannotBeProcessed(value, typeof(BudgetPermissionRequestId))
        ]);
        
        return new BudgetPermissionRequestId(value);
    }

    public static BudgetPermissionRequestId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetPermissionRequestId(value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}