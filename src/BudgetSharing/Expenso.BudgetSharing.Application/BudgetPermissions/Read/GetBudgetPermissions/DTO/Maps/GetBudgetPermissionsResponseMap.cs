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
            BudgetCode: budgetPermission.BudgetCode.Value,
            Permissions: budgetPermission.Permissions.Select(selector: MapTo).ToList());
    }

    private static GetBudgetPermissionsResponsePermission MapTo(Permission permission)
    {
        return new GetBudgetPermissionsResponsePermission(ParticipantId: permission.ParticipantId.Value,
            PermissionType: MapTo(permissionType: permission.PermissionType));
    }

    private static GetBudgetPermissionsResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.None)
        {
            return GetBudgetPermissionsResponsePermissionType.None;
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

        throw new ArgumentOutOfRangeException(paramName: nameof(permissionType), actualValue: permissionType,
            message: null);
    }
}