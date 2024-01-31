using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Requests.Maps;

public sealed class GetBudgetPermissionsRequestMap
{
    public static PermissionType MapTo(GetBudgetPermissionsRequestPermissionType permissionType)
    {
        return permissionType switch
        {
            GetBudgetPermissionsRequestPermissionType.Unknown => PermissionType.Unknown,
            GetBudgetPermissionsRequestPermissionType.Owner => PermissionType.Owner,
            GetBudgetPermissionsRequestPermissionType.SubOwner => PermissionType.SubOwner,
            GetBudgetPermissionsRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null)
        };
    }
}