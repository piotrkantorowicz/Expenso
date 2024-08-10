using Expenso.BudgetSharing.Proxy.DTO.API.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Request.Maps;

internal static class GetBudgetPermissionsExternalResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse>? MapTo(
        IEnumerable<GetBudgetPermissionsResponse>? budgetPermissions)
    {
        return budgetPermissions?.Select(selector: MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(GetBudgetPermissionsResponse budgetPermission)
    {
        return new GetBudgetPermissionsResponse(Id: budgetPermission.Id, BudgetId: budgetPermission.BudgetId,
            OwnerId: budgetPermission.OwnerId,
            Permissions: budgetPermission.Permissions.Select(selector: MapTo).ToList());
    }

    private static GetBudgetPermissionsResponse_Permission MapTo(GetBudgetPermissionsResponse_Permission permission)
    {
        return new GetBudgetPermissionsResponse_Permission(ParticipantId: permission.ParticipantId,
            PermissionType: MapTo(permissionType: permission.PermissionType));
    }

    private static GetBudgetPermissionsResponse_PermissionType MapTo(
        GetBudgetPermissionsResponse_PermissionType permissionType)
    {
        return permissionType;
    }
}