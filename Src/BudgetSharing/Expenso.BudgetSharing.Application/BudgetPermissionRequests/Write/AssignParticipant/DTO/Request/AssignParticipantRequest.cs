namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Request;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    Guid ParticipantId,
    AssignParticipantRequest_PermissionType PermissionType,
    int ExpirationDays);