namespace Expenso.BudgetSharing.Application.DTO.AssignParticipant;

public sealed record AssignParticipantRequest(Guid BudgetId, Guid ParticipantId, ParticipationType ParticipationType);