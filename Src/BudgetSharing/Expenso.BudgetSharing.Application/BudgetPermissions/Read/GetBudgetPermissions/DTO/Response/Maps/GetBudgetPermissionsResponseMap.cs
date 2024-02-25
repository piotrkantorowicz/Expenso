using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Response.Maps;

internal static class GetBudgetPermissionsResponseMap
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

    private static GetBudgetPermissionsResponse_Permission MapTo(Permission permission)
    {
        return new GetBudgetPermissionsResponse_Permission(permission.ParticipantId.Value,
            MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionsResponse_PermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionsResponse_PermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionsResponse_PermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionsResponse_PermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionsResponse_PermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}