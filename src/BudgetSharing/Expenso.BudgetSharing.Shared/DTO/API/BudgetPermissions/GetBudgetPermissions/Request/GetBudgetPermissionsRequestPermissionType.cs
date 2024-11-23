namespace Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;

[Flags]
public enum GetBudgetPermissionsRequestPermissionType
{
    None = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 4,
    All = Owner | SubOwner | Reviewer
}