using Expenso.BudgetSharing.Application.DTO.AssignParticipant;

using PermissionType = Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects.PermissionType;

namespace Expenso.BudgetSharing.Application.Mappings;

internal static class PermissionTypeMap
{
    public static PermissionType ToPermissionType(PermissionTypeRequest permissionTypeRequest)
    {
        return permissionTypeRequest switch
        {
            PermissionTypeRequest.Unknown => PermissionType.Unknown,
            PermissionTypeRequest.Owner => PermissionType.Owner,
            PermissionTypeRequest.SubOwner => PermissionType.SubOwner,
            PermissionTypeRequest.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(permissionTypeRequest), permissionTypeRequest, null)
        };
    }
}
