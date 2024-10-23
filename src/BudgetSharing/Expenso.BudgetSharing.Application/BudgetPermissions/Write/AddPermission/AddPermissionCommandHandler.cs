using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Maps;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;

internal sealed class AddPermissionCommandHandler : ICommandHandler<AddPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public AddPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));
    }

    public async Task HandleAsync(AddPermissionCommand command, CancellationToken cancellationToken)
    {
        BudgetPermission? budgetPermission = await _budgetPermissionRepository.GetByIdAsync(
            id: BudgetPermissionId.New(value: command.Payload?.BudgetPermissionId),
            cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(
                message: $"Budget permission with ID {command.Payload?.BudgetPermissionId} hasn't been found");
        }

        budgetPermission.AddPermission(participantId: PersonId.New(value: command.Payload?.ParticipantId),
            permissionType: AddPermissionRequestMap.ToPermissionType(
                addPermissionRequestPermissionType: command.Payload?.PermissionType));

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}