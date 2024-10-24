using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissions.GetBudgetPermissions.Response;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermissions.DTO.Maps;

internal static class GetBudgetPermissionsResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse> MapTo(
        IEnumerable<BudgetPermission> budgetPermissions)
    {
        return budgetPermissions.Select(selector: MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionsResponse(Id: budgetPermission.Id.Value,
            BudgetId: budgetPermission.BudgetId.Value, OwnerId: budgetPermission.OwnerId.Value,
            Permissions: budgetPermission.Permissions.Select(selector: MapTo).ToList());
    }

    private static GetBudgetPermissionsResponse_Permission MapTo(Permission permission)
    {
        return new GetBudgetPermissionsResponse_Permission(ParticipantId: permission.ParticipantId.Value,
            PermissionType: MapTo(permissionType: permission.PermissionType));
    }

    private static GetBudgetPermissionsResponse_PermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.None)
        {
            return GetBudgetPermissionsResponse_PermissionType.None;
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

        throw new ArgumentOutOfRangeException(paramName: nameof(permissionType), actualValue: permissionType,
            message: null);
    }
}