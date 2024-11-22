using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

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
            budgetPermissionId: BudgetPermissionId.New(value: command.Payload?.BudgetPermissionId),
            cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermission),
                identifierType: IdentifierType.PrimaryId(), identifier: command.Payload?.BudgetPermissionId);
        }

        budgetPermission.Unblock();

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}