using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

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
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinesRuleCheck(BusinessRule: new EmptyIdentifierCannotBeProcessed(identifier: value,
                type: typeof(BudgetPermissionRequestId)))
        ]);

        return new BudgetPermissionRequestId(value: value);
    }

    public static BudgetPermissionRequestId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetPermissionRequestId(value: value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}