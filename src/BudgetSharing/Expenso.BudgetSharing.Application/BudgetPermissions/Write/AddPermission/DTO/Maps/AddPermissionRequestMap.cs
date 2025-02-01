using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.AddPermission.Request;

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