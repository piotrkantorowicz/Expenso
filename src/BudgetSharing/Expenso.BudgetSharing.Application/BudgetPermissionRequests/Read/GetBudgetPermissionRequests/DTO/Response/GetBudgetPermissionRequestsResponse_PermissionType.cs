namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

[Flags]
public enum GetBudgetPermissionRequestsResponse_PermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4
}