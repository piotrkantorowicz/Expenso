namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

[Flags]
public enum GetBudgetPermissionRequestsResponse_Status
{
    None = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 4,
    Expired = 8,
    All = Pending | Confirmed | Cancelled | Expired
}