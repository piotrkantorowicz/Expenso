using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests.Maps;

internal sealed class AssignParticipantRequestMap
{
    public static PermissionType ToPermissionType(
        AssignParticipantRequestPermissionType assignParticipantRequestPermissionType)
    {
        return assignParticipantRequestPermissionType switch
        {
            AssignParticipantRequestPermissionType.Unknown => PermissionType.Unknown,
            AssignParticipantRequestPermissionType.Owner => PermissionType.Owner,
            AssignParticipantRequestPermissionType.SubOwner => PermissionType.SubOwner,
            AssignParticipantRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(assignParticipantRequestPermissionType),
                assignParticipantRequestPermissionType, null)
        };
    }
}