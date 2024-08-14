using Expenso.BudgetSharing.Domain.BudgetPermissionRequests;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissionRequests.Write.CancelAssigningParticipant;

internal sealed class CancelAssigningParticipantCommandHandler : ICommandHandler<CancelAssigningParticipantCommand>
{
    private readonly IBudgetPermissionRequestRepository _budgetPermissionRequestRepository;

    public CancelAssigningParticipantCommandHandler(
        IBudgetPermissionRequestRepository budgetPermissionRequestRepository)
    {
        _budgetPermissionRequestRepository = budgetPermissionRequestRepository ??
                                             throw new ArgumentNullException(
                                                 paramName: nameof(budgetPermissionRequestRepository));
    }

    public async Task HandleAsync(CancelAssigningParticipantCommand command, CancellationToken cancellationToken)
    {
        BudgetPermissionRequest? budgetPermissionRequest =
            await _budgetPermissionRequestRepository.GetByIdAsync(
                permissionId: BudgetPermissionRequestId.New(value: command.BudgetPermissionRequestId),
                cancellationToken: cancellationToken);

        if (budgetPermissionRequest is null)
        {
            throw new NotFoundException(
                message: $"Budget permission request with id {command.BudgetPermissionRequestId} hasn't been found");
        }

        budgetPermissionRequest.Cancel();

        await _budgetPermissionRequestRepository.UpdateAsync(permission: budgetPermissionRequest,
            cancellationToken: cancellationToken);
    }
}