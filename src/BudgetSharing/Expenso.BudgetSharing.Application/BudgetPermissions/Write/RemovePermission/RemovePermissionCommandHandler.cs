using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.RemovePermission;

internal sealed class RemovePermissionCommandHandler : ICommandHandler<RemovePermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public RemovePermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));
    }

    public async Task HandleAsync(RemovePermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission = await _budgetPermissionRepository.GetByIdAsync(
            budgetPermissionId: BudgetPermissionId.New(value: command.Payload?.BudgetPermissionId),
            cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(resourceName: nameof(BudgetPermission),
                identifierType: IdentifierType.PrimaryId(), identifier: command.Payload?.BudgetPermissionId);
        }

        budgetPermission.RemovePermission(participantId: PersonId.New(value: command.Payload?.ParticipantId));

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}