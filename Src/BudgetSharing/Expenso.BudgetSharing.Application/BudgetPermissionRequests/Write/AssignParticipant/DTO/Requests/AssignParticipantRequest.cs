namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.AssignParticipant.DTO.Requests;

public sealed record AssignParticipantRequest(
    Guid BudgetId,
    Guid ParticipantId,
    AssignParticipantRequestPermissionType PermissionType,
    int ExpirationDays);