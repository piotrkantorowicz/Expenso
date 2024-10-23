using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Services.Interfaces;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.ConfirmAssigningParticipant;

internal sealed class ConfirmAssigningParticipantCommandHandler : ICommandHandler<ConfirmAssigningParticipantCommand>
{
    private readonly IConfirmParticipantionDomainService _confirmParticipantionDomainService;

    public ConfirmAssigningParticipantCommandHandler(
        IConfirmParticipantionDomainService confirmParticipantionDomainService)
    {
        _confirmParticipantionDomainService = confirmParticipantionDomainService ??
                                              throw new ArgumentNullException(
                                                  paramName: nameof(confirmParticipantionDomainService));
    }

    public async Task HandleAsync(ConfirmAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        await _confirmParticipantionDomainService.ConfirmParticipantAsync(
            budgetPermissionRequestId: command.Payload?.BudgetPermissionRequestId,
            cancellationToken: cancellationToken);
    }
}