using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request.Maps;

internal sealed class AddPermissionRequestMap
{
    public static PermissionType ToPermissionType(AddPermissionRequestPermissionType addPermissionRequestPermissionType)
    {
        return addPermissionRequestPermissionType switch
        {
            AddPermissionRequestPermissionType.Unknown => PermissionType.Unknown,
            AddPermissionRequestPermissionType.Owner => PermissionType.Owner,
            AddPermissionRequestPermissionType.SubOwner => PermissionType.SubOwner,
            AddPermissionRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(addPermissionRequestPermissionType),
                addPermissionRequestPermissionType, null)
        };
    }
}