using Expenso.BudgetSharing.Application.DTO.AssignParticipant;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Mappings;

internal static class AccessTypeMap
{
    public static PermissionType ToParticipationType(ParticipationType participationType)
    {
        return participationType switch
        {
            ParticipationType.Unknown => PermissionType.Unknown,
            ParticipationType.Owner => PermissionType.Owner,
            ParticipationType.SubOwner => PermissionType.SubOwner,
            ParticipationType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(participationType), participationType, null)
        };
    }
}