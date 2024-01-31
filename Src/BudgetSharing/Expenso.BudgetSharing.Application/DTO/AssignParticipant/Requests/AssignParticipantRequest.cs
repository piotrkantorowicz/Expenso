namespace Expenso.BudgetSharing.Application.DTO.AssignParticipant.Requests;

public sealed record AssignParticipantRequest(
    Guid BudgetPermissionId,
    Guid ParticipantId,
    AssignParticipantRequestPermissionType AssignParticipantRequestPermissionType,
    int ExpirationDays);