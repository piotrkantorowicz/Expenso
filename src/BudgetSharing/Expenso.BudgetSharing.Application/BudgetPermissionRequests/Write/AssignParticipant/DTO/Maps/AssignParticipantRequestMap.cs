using Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Maps;

internal sealed class AssignParticipantRequestMap
{
    public static PermissionType? ToPermissionType(
        AssignParticipantRequest_PermissionType? assignParticipantRequestPermissionType)
    {
        return assignParticipantRequestPermissionType switch
        {
            AssignParticipantRequest_PermissionType.None => PermissionType.None,
            AssignParticipantRequest_PermissionType.Owner => PermissionType.Owner,
            AssignParticipantRequest_PermissionType.SubOwner => PermissionType.SubOwner,
            AssignParticipantRequest_PermissionType.Reviewer => PermissionType.Reviewer,
            _ => null
        };
    }
}