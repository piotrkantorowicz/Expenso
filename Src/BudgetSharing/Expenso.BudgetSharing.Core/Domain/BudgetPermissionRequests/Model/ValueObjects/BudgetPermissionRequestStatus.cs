using Expenso.Shared.Types.Domain;

namespace Expenso.BudgetSharing.Core.Domain.BudgetPermissionRequests.Model.ValueObjects;

internal sealed record BudgetPermissionRequestStatus(int Value, string DisplayName)
    : Enumeration<BudgetPermissionRequestStatus>(Value, DisplayName)
{
    public static readonly BudgetPermissionRequestStatus Unknown = new(0, "Unknown");
    public static readonly BudgetPermissionRequestStatus Pending = new(1, "Pending");
    public static readonly BudgetPermissionRequestStatus Confirmed = new(2, "Confirmed");
    public static readonly BudgetPermissionRequestStatus Cancelled = new(3, "Cancelled");
    public static readonly BudgetPermissionRequestStatus Expired = new(4, "Expired");

    public bool IsPending()
    {
        return this == Pending;
    }
}