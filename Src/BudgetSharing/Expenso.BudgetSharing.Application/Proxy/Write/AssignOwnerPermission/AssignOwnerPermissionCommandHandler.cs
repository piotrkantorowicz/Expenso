using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Proxy.Write.AssignOwnerPermission;

internal sealed class AssignOwnerPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<AssignOwnerPermissionCommand, AssignOwnerPermissionResponse>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task<AssignOwnerPermissionResponse?> HandleAsync(AssignOwnerPermissionCommand command,
        CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionId, (Guid budgetId, Guid ownerId)) = command;

        BudgetPermission budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(BudgetPermissionId.New(budgetPermissionId),
                cancellationToken) ?? BudgetPermission.Create(BudgetId.New(budgetId), PersonId.New(ownerId));

        budgetPermission.AddPermission(PersonId.New(ownerId), PermissionType.Owner);
        await _budgetPermissionRepository.AddOrUpdateAsync(budgetPermission, cancellationToken);

        return new AssignOwnerPermissionResponse(budgetPermission.Id.Value);
    }
}