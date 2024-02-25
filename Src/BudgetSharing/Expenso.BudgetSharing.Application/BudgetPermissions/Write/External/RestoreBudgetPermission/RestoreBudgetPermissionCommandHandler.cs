using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.RestoreBudgetPermission;

internal sealed class RestoreBudgetPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<RestoreBudgetPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task HandleAsync(RestoreBudgetPermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(BudgetPermissionId.New(command.BudgetPermissionId),
                cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {command.BudgetPermissionId} hasn't been found.");
        }

        budgetPermission.Restore();
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}