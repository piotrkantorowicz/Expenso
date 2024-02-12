using Expenso.BudgetSharing.Domain.Shared.Model.Base;
using Expenso.BudgetSharing.Domain.Shared.Model.Rules;

namespace Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

public sealed class BudgetId
{
    private BudgetId(Guid value)
    {
        DomainModelState.CheckBusinessRules([new EmptyIdentifierCannotBeProcessed(value, GetType())]);
        Value = value;
    }

    public Guid Value { get; }

    public static BudgetId Create(Guid value)
    {
        return new BudgetId(value);
    }

    public override string ToString()
    {
        return Value.ToString();
    }
}