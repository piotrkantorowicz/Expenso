namespace Expenso.BudgetSharing.Core.Application.DTO.AssignParticipant;

public sealed record AssignParticipantRequest(Guid BudgetId, Guid ParticipantId, ParticipationType ParticipationType);