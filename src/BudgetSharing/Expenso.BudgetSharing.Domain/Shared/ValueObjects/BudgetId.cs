using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;
using Expenso.Shared.Domain.Types.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.ValueObjects;

public sealed record BudgetId
{
    private BudgetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetId New(Guid? value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
        [
            new BusinessRuleCheck(
                BusinessRule: new EmptyIdentifierCannotBeProcessed(identifier: value, type: typeof(BudgetId)))
        ]);

        return new BudgetId(value: value!.Value);
    }

    public static BudgetId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetId(value: value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}