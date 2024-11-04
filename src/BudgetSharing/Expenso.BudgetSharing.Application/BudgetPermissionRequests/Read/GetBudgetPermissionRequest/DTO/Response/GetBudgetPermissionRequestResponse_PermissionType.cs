namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequest.DTO.Response;

[Flags]
public enum GetBudgetPermissionRequestResponse_PermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4,
    All = Owner | SubOwner | Reviewer
}