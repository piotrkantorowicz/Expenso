using Expenso.Shared.Domain.Types.ValueObjects;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Model.ValueObjects;

public sealed record BudgetPermissionRequestStatus : Enumeration<BudgetPermissionRequestStatus>
{
    public static readonly BudgetPermissionRequestStatus Unknown = new(0, "Unknown");
    public static readonly BudgetPermissionRequestStatus Pending = new(1, "Pending");
    public static readonly BudgetPermissionRequestStatus Confirmed = new(2, "Confirmed");
    public static readonly BudgetPermissionRequestStatus Cancelled = new(3, "Cancelled");
    public static readonly BudgetPermissionRequestStatus Expired = new(4, "Expired");

    private BudgetPermissionRequestStatus(int value, string displayName) : base(value, displayName)
    {
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