using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;

internal sealed class DeleteBudgetPermissionCommandHandler(
    IBudgetPermissionRepository budgetPermissionRepository,
    IClock clock) : ICommandHandler<DeleteBudgetPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    private readonly IClock _clock = clock ?? throw new ArgumentNullException(nameof(clock));

    public async Task HandleAsync(DeleteBudgetPermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(BudgetPermissionId.New(command.BudgetPermissionId),
                cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {command.BudgetPermissionId} hasn't been found.");
        }

        budgetPermission.Delete(_clock);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}