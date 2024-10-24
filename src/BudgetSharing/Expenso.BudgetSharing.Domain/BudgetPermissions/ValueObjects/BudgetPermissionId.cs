using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;

public sealed record BudgetPermissionId
{
    private BudgetPermissionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetPermissionId New(Guid? value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new EmptyIdentifierCannotBeProcessed(identifier: value, type: typeof(BudgetPermissionId)))
        ]);

        return new BudgetPermissionId(value: value!.Value);
    }

    public static BudgetPermissionId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetPermissionId(value: value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}