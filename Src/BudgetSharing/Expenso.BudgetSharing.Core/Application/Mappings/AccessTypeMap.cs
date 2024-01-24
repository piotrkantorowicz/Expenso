using Expenso.BudgetSharing.Core.Application.DTO.AssignParticipant;
using Expenso.BudgetSharing.Core.Domain.BudgetPermissions.Model.ValueObjects;

namespace Expenso.BudgetSharing.Core.Application.Mappings;

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