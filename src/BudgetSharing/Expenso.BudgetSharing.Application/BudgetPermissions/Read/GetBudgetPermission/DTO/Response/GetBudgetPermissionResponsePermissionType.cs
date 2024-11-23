namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

[Flags]
public enum GetBudgetPermissionResponsePermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4,
    All = Owner | SubOwner | Reviewer
}