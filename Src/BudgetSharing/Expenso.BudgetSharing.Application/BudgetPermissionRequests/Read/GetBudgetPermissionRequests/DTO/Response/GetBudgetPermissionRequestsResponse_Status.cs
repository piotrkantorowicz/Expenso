namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

public enum GetBudgetPermissionRequestsResponse_Status
{
    Unknown = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Expired = 4
}