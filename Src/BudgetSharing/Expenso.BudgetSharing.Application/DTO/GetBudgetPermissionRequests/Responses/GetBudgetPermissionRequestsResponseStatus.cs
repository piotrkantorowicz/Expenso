namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequests.Responses;

public enum GetBudgetPermissionRequestsResponseStatus
{
    Unknown = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Expired = 4
}