namespace Expenso.BudgetSharing.Domain.Shared;

public sealed class BudgetId
{
    private BudgetId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(BudgetId id)
    {
        return id.Value;
    }

    public static implicit operator BudgetId(Guid id)
    {
        return new BudgetId(id);
    }

    public static BudgetId CreateDefault()
    {
        return new BudgetId(Guid.Empty);
    }

    public static BudgetId Create(Guid value)
    {
        return new BudgetId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}
