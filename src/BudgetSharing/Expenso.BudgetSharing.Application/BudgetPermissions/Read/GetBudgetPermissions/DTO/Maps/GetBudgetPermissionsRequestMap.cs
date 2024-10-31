using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;

public static class GetBudgetPermissionsRequestMap
{
    public static PermissionType[] MapTo(GetBudgetPermissionsRequest_PermissionType? permissionType)
    {
        if (permissionType is null or GetBudgetPermissionsRequest_PermissionType.None)
        {
            return [PermissionType.None];
        }

        ICollection<PermissionType> permissionTypes = [];

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionsRequest_PermissionType.Owner))
        {
            permissionTypes.Add(item: PermissionType.Owner);
        }

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionsRequest_PermissionType.SubOwner))
        {
            permissionTypes.Add(item: PermissionType.SubOwner);
        }

        if (permissionType.Value.HasFlag(flag: GetBudgetPermissionsRequest_PermissionType.Reviewer))
        {
            permissionTypes.Add(item: PermissionType.Reviewer);
        }

        return permissionTypes.ToArray();
    }
}