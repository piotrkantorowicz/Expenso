namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

[Flags]
public enum GetBudgetPermissionRequestResponseStatus
{
    None = 0,
    Pending = 1,
    Confirmed = 2,
    Cancelled = 4,
    Expired = 8,
    All = Pending | Confirmed | Cancelled | Expired
}