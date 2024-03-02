using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;

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
            (new EmptyIdentifierCannotBeProcessed(value, typeof(BudgetPermissionRequestId)), false)
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