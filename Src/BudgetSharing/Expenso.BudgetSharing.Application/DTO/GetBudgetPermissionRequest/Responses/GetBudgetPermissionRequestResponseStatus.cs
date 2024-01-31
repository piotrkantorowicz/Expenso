namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissionRequest.Responses;

public enum GetBudgetPermissionRequestResponseStatus
{
    Unknown = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 3,
    Expired = 4
}