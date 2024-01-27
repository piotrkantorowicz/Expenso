namespace Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

public sealed class PermissionId
{
    private PermissionId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(PermissionId id)
    {
        return id.Value;
    }

    public static implicit operator PermissionId(Guid id)
    {
        return new PermissionId(id);
    }

    public static PermissionId CreateDefault()
    {
        return new PermissionId(Guid.Empty);
    }

    public static PermissionId Create(Guid value)
    {
        return new PermissionId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}
