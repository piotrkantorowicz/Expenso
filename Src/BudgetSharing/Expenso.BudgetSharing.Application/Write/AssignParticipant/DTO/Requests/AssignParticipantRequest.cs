namespace Expenso.BudgetSharing.Application.Write.AssignParticipant.DTO.Requests;

public sealed record AssignParticipantRequest(
    Guid BudgetPermissionId,
    Guid ParticipantId,
    AssignParticipantRequestPermissionType AssignParticipantRequestPermissionType,
    int ExpirationDays);