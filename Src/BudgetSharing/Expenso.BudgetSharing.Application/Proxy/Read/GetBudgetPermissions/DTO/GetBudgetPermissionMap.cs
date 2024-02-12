using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Responses;

namespace Expenso.BudgetSharing.Application.Proxy.Read.GetBudgetPermissions.DTO;

internal static class GetBudgetPermissionMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse> MapTo(
        IEnumerable<BudgetPermission> budgetPermissions)
    {
        return budgetPermissions.Select(MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionsResponse(budgetPermission.Id.Value, budgetPermission.BudgetId.Value,
            budgetPermission.OwnerId.Value, budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionsResponsePermission MapTo(Permission permission)
    {
        return new GetBudgetPermissionsResponsePermission(permission.ParticipantId.Value,
            MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionsResponsePermissionType MapTo(PermissionType permissionType)
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