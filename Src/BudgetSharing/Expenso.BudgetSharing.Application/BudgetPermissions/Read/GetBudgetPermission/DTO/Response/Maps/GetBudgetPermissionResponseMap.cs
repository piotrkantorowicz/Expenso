using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Read.GetBudgetPermission.DTO.Response.Maps;

internal static class GetBudgetPermissionResponseMap
{
    public static GetBudgetPermissionResponse MapTo(BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionResponse(budgetPermission.Id.Value, budgetPermission.BudgetId.Value,
            budgetPermission.OwnerId.Value, budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionResponse_Permission MapTo(Permission permission)
    {
        return new GetBudgetPermissionResponse_Permission(permission.ParticipantId.Value,
            MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionResponse_PermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionResponse_PermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionResponse_PermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionResponse_PermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionResponse_PermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}