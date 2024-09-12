namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response;

[Flags]
public enum GetBudgetPermissionResponse_PermissionType
{
    Unknown = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 3
}