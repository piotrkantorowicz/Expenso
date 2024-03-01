using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;

public sealed class GetBudgetPermissionsRequestMap
{
    public static PermissionType MapTo(GetBudgetPermissionsRequest_PermissionType permissionType)
    {
        return permissionType switch
        {
            GetBudgetPermissionsRequest_PermissionType.Unknown => PermissionType.Unknown,
            GetBudgetPermissionsRequest_PermissionType.Owner => PermissionType.Owner,
            GetBudgetPermissionsRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            GetBudgetPermissionsRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null)
        };
    }
}