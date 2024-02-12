using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.Write.RemovePermission;

internal sealed class RemovePermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<RemovePermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task HandleAsync(RemovePermissionCommand command, CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionId, Guid participantId) = command;

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(BudgetPermissionId.New(budgetPermissionId),
                cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {budgetPermissionId} hasn't been found");
        }

        budgetPermission.RemovePermission(PersonId.New(participantId));
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}