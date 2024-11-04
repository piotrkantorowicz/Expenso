using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;

public static class GetBudgetPermissionsRequestMap
{
    private static readonly Dictionary<GetBudgetPermissionsRequest_PermissionType, PermissionType> PermissionTypeMap =
        new()
        {
            { GetBudgetPermissionsRequest_PermissionType.Owner, PermissionType.Owner },
            { GetBudgetPermissionsRequest_PermissionType.SubOwner, PermissionType.SubOwner },
            { GetBudgetPermissionsRequest_PermissionType.Reviewer, PermissionType.Reviewer }
        };

    public static PermissionType[] MapTo(GetBudgetPermissionsRequest_PermissionType? permissionType)
    {
        return permissionType is null or GetBudgetPermissionsRequest_PermissionType.None
            ? ( [PermissionType.None])
            : PermissionTypeMap
                .Where(predicate: kv => permissionType.Value.HasFlag(flag: kv.Key))
                .Select(selector: kv => kv.Value)
                .ToArray();
    }
}