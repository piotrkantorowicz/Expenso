using Expenso.BudgetSharing.Application.DTO.RemoveParticipant.Requests;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Commands.RemoveParticipant;

public sealed record RemoveParticipantCommand(
    Guid BudgetPermissionId,
    RemoveParticipantRequest RemoveParticipantRequest) : ICommand;