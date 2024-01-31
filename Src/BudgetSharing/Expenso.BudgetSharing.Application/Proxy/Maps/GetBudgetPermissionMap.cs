using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

namespace Expenso.BudgetSharing.Application.Proxy.Maps;

internal static class GetBudgetPermissionMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse> MapTo(
        this IEnumerable<BudgetPermission> budgetPermissions)
    {
        return budgetPermissions.Select(MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(this BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionsResponse(budgetPermission.Id, budgetPermission.BudgetId,
            budgetPermission.OwnerId, budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionsResponsePermission MapTo(this Permission permission)
    {
        return new GetBudgetPermissionsResponsePermission(permission.Id, permission.BudgetPermissionId,
            permission.ParticipantId, MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionsResponsePermissionType MapTo(this PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionsResponsePermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionsResponsePermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionsResponsePermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionsResponsePermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}