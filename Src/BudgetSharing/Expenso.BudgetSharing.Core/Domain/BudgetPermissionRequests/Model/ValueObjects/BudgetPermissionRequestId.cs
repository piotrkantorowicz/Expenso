namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;

internal sealed class BudgetPermissionRequestId
{
    private BudgetPermissionRequestId(Guid value)
    {
        Value = value;
    }

    public Guid Value { get; }

    public static implicit operator Guid(BudgetPermissionRequestId requestId)
    {
        return requestId.Value;
    }

    public static implicit operator BudgetPermissionRequestId(Guid id)
    {
        return new BudgetPermissionRequestId(id);
    }

    public static BudgetPermissionRequestId CreateDefault()
    {
        return new BudgetPermissionRequestId(Guid.Empty);
    }

    public static BudgetPermissionRequestId Create(Guid value)
    {
        return new BudgetPermissionRequestId(value);
    }

    public bool IsEmpty()
    {
        return Value == Guid.Empty;
    }
}