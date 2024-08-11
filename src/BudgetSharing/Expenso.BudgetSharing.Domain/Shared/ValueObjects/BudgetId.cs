using Expenso.BudgetSharing.Domain.Shared.Rules;
using Expenso.Shared.Domain.Types.Model;

namespace Expenso.BudgetSharing.Domain.Shared.ValueObjects;

public sealed record BudgetId
{
    private BudgetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetId New(Guid value)
    {
        DomainModelState.CheckBusinessRules(businessRules:
            [(new EmptyIdentifierCannotBeProcessed(identifier: value, type: typeof(BudgetId)), false)]);

        return new BudgetId(value: value);
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