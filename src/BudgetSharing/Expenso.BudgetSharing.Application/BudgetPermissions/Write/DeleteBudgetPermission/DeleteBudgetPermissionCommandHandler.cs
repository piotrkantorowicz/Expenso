using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.DeleteBudgetPermission;

internal sealed class DeleteBudgetPermissionCommandHandler : ICommandHandler<DeleteBudgetPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;
    private readonly IClock _clock;

    public DeleteBudgetPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository, IClock clock)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));

        _clock = clock ?? throw new ArgumentNullException(paramName: nameof(clock));
    }

    public async Task HandleAsync(DeleteBudgetPermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission = await _budgetPermissionRepository.GetByIdAsync(
            budgetPermissionId: BudgetPermissionId.New(value: command.Payload?.BudgetPermissionId),
            cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermission),
                identifierType: IdentifierType.PrimaryId(), identifier: command.Payload?.BudgetPermissionId);
        }

        budgetPermission.Block(clock: _clock);

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}