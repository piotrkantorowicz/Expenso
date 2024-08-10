using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission.DTO.Request.Maps;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.AddPermission;

internal sealed class AddPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<AddPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   paramName: nameof(
                                                                                       budgetPermissionRepository));

    public async Task HandleAsync(AddPermissionCommand command, CancellationToken cancellationToken)
    {
        (_, Guid budgetPermissionId, Guid participantId, AddPermissionRequest addPermissionRequest) = command;

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(id: BudgetPermissionId.New(value: budgetPermissionId),
                cancellationToken: cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException(message: $"Budget permission with id {budgetPermissionId} hasn't been found.");
        }

        budgetPermission.AddPermission(participantId: PersonId.New(value: participantId),
            permissionType: AddPermissionRequestMap.ToPermissionType(
                addPermissionRequestPermissionType: addPermissionRequest.PermissionType));

        await _budgetPermissionRepository.UpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);
    }
}