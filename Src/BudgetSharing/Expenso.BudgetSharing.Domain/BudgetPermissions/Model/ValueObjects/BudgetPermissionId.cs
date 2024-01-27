namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

public sealed class BudgetPermissionId
{
    private BudgetPermissionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(BudgetPermissionId id)
    {
        return id.Value;
    }

    public static implicit operator BudgetPermissionId(Guid id)
    {
        return new BudgetPermissionId(id);
    }

    public static BudgetPermissionId CreateDefault()
    {
        return new BudgetPermissionId(Guid.Empty);
    }

    public static BudgetPermissionId Create(Guid value)
    {
        return new BudgetPermissionId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}
