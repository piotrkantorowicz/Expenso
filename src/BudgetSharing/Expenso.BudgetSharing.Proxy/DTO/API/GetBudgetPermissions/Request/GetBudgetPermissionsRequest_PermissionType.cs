namespace Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;

[Flags]
public enum GetBudgetPermissionsRequest_PermissionType
{
    Unknown = 0,
    Owner = 1,
    SubOwner = 2,
    Reviewer = 3
}