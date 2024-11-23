using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Maps;

internal sealed class AddPermissionRequestMap
{
    public static PermissionType? ToPermissionType(
        AddPermissionRequestPermissionType? addPermissionRequestPermissionType)
    {
        return addPermissionRequestPermissionType switch
        {
            AddPermissionRequestPermissionType.None => PermissionType.None,
            AddPermissionRequestPermissionType.Owner => PermissionType.Owner,
            AddPermissionRequestPermissionType.SubOwner => PermissionType.SubOwner,
            AddPermissionRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => null
        };
    }
}