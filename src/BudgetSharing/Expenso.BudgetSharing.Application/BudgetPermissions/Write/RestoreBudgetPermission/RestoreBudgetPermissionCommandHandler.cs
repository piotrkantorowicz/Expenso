using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.RestoreBudgetPermission;

internal sealed class RestoreBudgetPermissionCommandHandler : ICommandHandler<RestoreBudgetPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public RestoreBudgetPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));
    }

    public async Task HandleAsync(RestoreBudgetPermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission = await _budgetPermissionRepository.GetByIdAsync(
            id: BudgetPermissionId.New(value: command.BudgetPermissionId), cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(
                message: $"Budget permission with id {command.BudgetPermissionId} hasn't been found");
        }

        budgetPermission.Unblock();

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}