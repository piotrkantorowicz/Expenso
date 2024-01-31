using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.DTO.GetBudgetPermissions.Responses.Maps;

internal sealed class GetBudgetPermissionsResponseMap
{
    public static IReadOnlyCollection<GetBudgetPermissionsResponse> MapTo(
        IEnumerable<BudgetPermission> budgetPermissions)
    {
        return budgetPermissions.Select(MapTo).ToList();
    }

    private static GetBudgetPermissionsResponse MapTo(BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionsResponse(budgetPermission.Id, budgetPermission.BudgetId,
            budgetPermission.OwnerId, budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionsResponsePermission MapTo(Permission permission)
    {
        return new GetBudgetPermissionsResponsePermission(permission.Id, permission.BudgetPermissionId,
            permission.ParticipantId, MapTo(permission.PermissionType));
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