using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Read.GetBudgetPermission.DTO.Responses.Maps;

internal sealed class GetBudgetPermissionResponseMap
{
    public static GetBudgetPermissionResponse MapTo(BudgetPermission budgetPermission)
    {
        return new GetBudgetPermissionResponse(budgetPermission.Id, budgetPermission.BudgetId, budgetPermission.OwnerId,
            budgetPermission.Permissions.Select(MapTo).ToList());
    }

    private static GetBudgetPermissionResponsePermission MapTo(Permission permission)
    {
        return new GetBudgetPermissionResponsePermission(permission.ParticipantId, MapTo(permission.PermissionType));
    }

    private static GetBudgetPermissionResponsePermissionType MapTo(PermissionType permissionType)
    {
        if (permissionType == PermissionType.Unknown)
        {
            return GetBudgetPermissionResponsePermissionType.Unknown;
        }

        if (permissionType == PermissionType.Owner)
        {
            return GetBudgetPermissionResponsePermissionType.Owner;
        }

        if (permissionType == PermissionType.SubOwner)
        {
            return GetBudgetPermissionResponsePermissionType.SubOwner;
        }

        if (permissionType == PermissionType.Reviewer)
        {
            return GetBudgetPermissionResponsePermissionType.Reviewer;
        }

        throw new ArgumentOutOfRangeException(nameof(permissionType), permissionType, null);
    }
}