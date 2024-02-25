using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request.Maps;

internal sealed class AssignParticipantRequestMap
{
    public static PermissionType ToPermissionType(
        AssignParticipantRequest_PermissionType assignParticipantRequestPermissionType)
    {
        return assignParticipantRequestPermissionType switch
        {
            AssignParticipantRequest_PermissionType.Unknown => PermissionType.Unknown,
            AssignParticipantRequest_PermissionType.Owner => PermissionType.Owner,
            AssignParticipantRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            AssignParticipantRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => throw new ArgumentOutOfRangeException(nameof(assignParticipantRequestPermissionType),
                assignParticipantRequestPermissionType, null)
        };
    }
}