using Expenso.BudgetSharing.Domain.BudgetPermissions.Model;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.Internal.Commands.AssignOwnerPermission;

internal sealed class AssignOwnerPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<AssignOwnerPermissionCommand,
        BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses.AssignOwnerPermission>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task<BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses.AssignOwnerPermission?> HandleAsync(
        AssignOwnerPermissionCommand command, CancellationToken cancellationToken = default)
    {
        (Guid budgetPermissionId, (Guid budgetId, Guid ownerId)) = command;

        BudgetPermission budgetPermission =
            await _budgetPermissionRepository.GetByIdAsync(budgetPermissionId, cancellationToken) ??
            BudgetPermission.Create(budgetId, ownerId);

        budgetPermission.AddPermission(ownerId, PermissionType.Owner);
        await _budgetPermissionRepository.AddOrUpdateAsync(budgetPermission, cancellationToken);

        return new BudgetSharing.Proxy.DTO.API.AssignOwnerPermission.Responses.AssignOwnerPermission(
            budgetPermission.Id);
    }
}