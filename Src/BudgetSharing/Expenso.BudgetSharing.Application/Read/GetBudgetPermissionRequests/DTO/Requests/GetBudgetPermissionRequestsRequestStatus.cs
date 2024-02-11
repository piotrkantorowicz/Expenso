namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermissionRequests.DTO.Requests;

public enum GetBudgetPermissionRequestsRequestStatus
{
    Unknown = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Expired = 4
}