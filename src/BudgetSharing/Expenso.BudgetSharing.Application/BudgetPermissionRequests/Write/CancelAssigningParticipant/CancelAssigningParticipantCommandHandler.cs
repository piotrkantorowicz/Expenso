using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Time;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;

internal sealed class CancelAssigningParticipantCommandHandler : ICommandHandler<CancelAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;
    private readonly IClock _clock;

    public CancelAssigningParticipantCommandHandler(
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository, IClock clock)
    {
        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));

        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public async Task HandleAsync(CancelAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(
                permissionId: BudgetPermissionRequestId.New(value: command.Payload?.BudgetPermissionRequestId),
                cancellationToken: cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermissionRequest),
                identifierType: IdentifierType.PrimaryId(), identifier: command.Payload?.BudgetPermissionRequestId);
        }

        budgetPermissionRequest.Cancel(cancellationDate: _clock.UtcNow);

        await _budgetPermissionRequestRepository.UpdateAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);
    }
}