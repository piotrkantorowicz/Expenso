using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Maps;

internal sealed class AddPermissionRequestMap
{
    public static PermissionType? ToPermissionType(
        AddPermissionRequest_PermissionType? addPermissionRequestPermissionType)
    {
        return addPermissionRequestPermissionType switch
        {
            AddPermissionRequest_PermissionType.None => PermissionType.None,
            AddPermissionRequest_PermissionType.Owner => PermissionType.Owner,
            AddPermissionRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            AddPermissionRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => null
        };
    }
}