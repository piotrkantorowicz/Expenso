using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed record BudgetId
{
    private BudgetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetId New(Guid value)
    {
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, typeof(BudgetId))]);

        return new BudgetId(value);
    }

    public static BudgetId? Nullable(Guid? value)
    {
        return value is null || value == Guid.Empty ? null : new BudgetId(value.Value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}