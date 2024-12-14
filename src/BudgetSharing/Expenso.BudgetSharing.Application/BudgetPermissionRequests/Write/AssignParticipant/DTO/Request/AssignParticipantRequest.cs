namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    string Email,
    AssignParticipantRequestPermissionType PermissionType,
    Guid? BudgetPermissionRequestId = null);