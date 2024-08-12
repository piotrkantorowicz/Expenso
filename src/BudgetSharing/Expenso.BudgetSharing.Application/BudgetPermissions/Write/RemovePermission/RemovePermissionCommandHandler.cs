using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission;

internal sealed class RemovePermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<RemovePermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   paramName: nameof(
                                                                                       budgetPermissionRepository));

    public async Task HandleAsync(RemovePermissionCommand command, CancellationToken cancellationToken)
    {
        (_, Guid budgetPermissionId, Guid participantId) = command;

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(id: BudgetPermissionId.New(value: budgetPermissionId),
                cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(message: $"Budget permission with id {budgetPermissionId} hasn't been found");
        }

        budgetPermission.RemovePermission(participantId: PersonId.New(value: participantId));

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}