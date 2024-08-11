using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;

public sealed record BudgetPermissionRequestStatus : Enumeration<BudgetPermissionRequestStatus>
{
    public static readonly BudgetPermissionRequestStatus Unknown = new(value: 0, displayName: "Unknown");
    public static readonly BudgetPermissionRequestStatus Pending = new(value: 1, displayName: "Pending");
    public static readonly BudgetPermissionRequestStatus Confirmed = new(value: 2, displayName: "Confirmed");
    public static readonly BudgetPermissionRequestStatus Cancelled = new(value: 3, displayName: "Cancelled");
    public static readonly BudgetPermissionRequestStatus Expired = new(value: 4, displayName: "Expired");

    private BudgetPermissionRequestStatus(int value, string displayName) : base(Value: value, DisplayName: displayName)
    {
    }

    public static BudgetPermissionRequestStatus Create(int value)
    {
        return FromValue(value: value);
    }

    public bool IsPending()
    {
        return this == Pending;
    }

    public bool IsUnknown()
    {
        return this == Unknown;
    }
}