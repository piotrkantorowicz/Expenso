namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    string Email,
    AssignParticipantRequest_PermissionType PermissionType);