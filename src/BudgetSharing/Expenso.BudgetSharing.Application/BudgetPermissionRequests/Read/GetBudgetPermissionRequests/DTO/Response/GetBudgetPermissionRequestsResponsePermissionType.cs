namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Read.GetBudgetPermissionRequests.DTO.Response;

[Flags]
public enum GetBudgetPermissionRequestsResponsePermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4,
    All = Owner | SubOwner | Reviewer
}