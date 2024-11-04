namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Request;

[Flags]
public enum GetBudgetPermissionRequestsRequest_PermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4,
    All = Owner | SubOwner | Reviewer
}