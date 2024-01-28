namespace Expenso.BudgetSharing.Application.DTO.AssignParticipant;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    Guid ParticipantId,
    PermissionTypeRequest PermissionTypeRequest);
