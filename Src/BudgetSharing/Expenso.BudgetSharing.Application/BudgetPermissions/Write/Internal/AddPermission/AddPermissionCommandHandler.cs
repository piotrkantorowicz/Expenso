using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.AddPermission.DTO.Request;
using Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.AddPermission.DTO.Request.Maps;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Commands;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.Internal.AddPermission;

internal sealed class AddPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<AddPermissionCommand>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task HandleAsync(AddPermissionCommand command, CancellationToken cancellationToken)
    {
        (_, Guid budgetPermissionId, Guid participantId, AddPermissionRequest addPermissionRequest) = command;

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(BudgetPermissionId.New(budgetPermissionId),
                cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission with id {budgetPermissionId} hasn't been found.");
        }

        budgetPermission.AddPermission(PersonId.New(participantId),
            AddPermissionRequestMap.ToPermissionType(addPermissionRequest.PermissionType));

        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
    }
}