using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Shared.DTO.API.BudgetPermissionRequests.AssignParticipant.Request;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Maps;

internal sealed class AssignParticipantRequestMap
{
    public static PermissionType? ToPermissionType(
        AssignParticipantRequestPermissionType? assignParticipantRequestPermissionType)
    {
        return assignParticipantRequestPermissionType switch
        {
            AssignParticipantRequestPermissionType.None => PermissionType.None,
            AssignParticipantRequestPermissionType.Owner => PermissionType.Owner,
            AssignParticipantRequestPermissionType.SubOwner => PermissionType.SubOwner,
            AssignParticipantRequestPermissionType.Reviewer => PermissionType.Reviewer,
            _ => null
        };
    }
}