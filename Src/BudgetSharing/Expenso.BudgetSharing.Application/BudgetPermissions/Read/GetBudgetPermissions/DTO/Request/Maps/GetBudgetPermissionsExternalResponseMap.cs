using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;

internal static class GetBudgetPermissionsExternalResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse>? MapTo(
        IEnumerable<GetBudgetPermissionsResponse>? budgetPermissions)
    {
        return budgetPermissions?.Select(MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(GetBudgetPermissionsResponse budgetPermission)
    {
        return new GetBudgetPermissionsResponse(budgetPermission.Id, budgetPermission.BudgetId,
            budgetPermission.OwnerId, budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionsResponse_Permission MapTo(GetBudgetPermissionsResponse_Permission permission)
    {
        return new GetBudgetPermissionsResponse_Permission(permission.ParticipantId, MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionsResponse_PermissionType MapTo(
        GetBudgetPermissionsResponse_PermissionType permissionType)
    {
        return permissionType;
    }
}