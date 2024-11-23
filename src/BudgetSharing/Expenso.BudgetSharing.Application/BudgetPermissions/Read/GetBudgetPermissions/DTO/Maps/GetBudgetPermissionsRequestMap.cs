using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;

public static class GetBudgetPermissionsRequestMap
{
    private static readonly Dictionary<GetBudgetPermissionsRequestPermissionType, PermissionType> PermissionTypeMap =
        new()
        {
            { GetBudgetPermissionsRequestPermissionType.Owner, PermissionType.Owner },
            { GetBudgetPermissionsRequestPermissionType.SubOwner, PermissionType.SubOwner },
            { GetBudgetPermissionsRequestPermissionType.Reviewer, PermissionType.Reviewer }
        };

    public static PermissionType[] MapTo(GetBudgetPermissionsRequestPermissionType? permissionType)
    {
        return permissionType is null or GetBudgetPermissionsRequestPermissionType.None
            ? ( [PermissionType.None])
            : PermissionTypeMap
                .Where(predicate: kv => permissionType.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }
}