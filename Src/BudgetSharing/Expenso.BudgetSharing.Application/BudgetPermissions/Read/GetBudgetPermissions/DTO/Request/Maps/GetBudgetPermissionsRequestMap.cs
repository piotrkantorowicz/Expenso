using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;

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