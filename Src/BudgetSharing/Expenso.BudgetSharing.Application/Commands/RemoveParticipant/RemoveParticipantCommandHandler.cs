using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Commands.RemoveParticipant;

internal sealed class RemoveParticipantCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<RemoveParticipantCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task HandleAsync(RemoveParticipantCommand command, CancellationToken cancellationToken = default)
    {
        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(command.BudgetPermissionId, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {command.BudgetPermissionId} hasn't been found");
        }

        budgetPermission.RemovePermission(command.RemoveParticipantRequest.PermissionId);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}