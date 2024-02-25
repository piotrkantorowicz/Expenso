using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.Model.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Responses;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.External.CreateBudgetPermission;

internal sealed class CreateBudgetPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    : ICommandHandler<CreateBudgetPermissionCommand, CreateBudgetPermissionResponse>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    public async Task<CreateBudgetPermissionResponse?> HandleAsync(CreateBudgetPermissionCommand command,
        CancellationToken cancellationToken)
    {
        (_, (Guid? budgetPermissionId, Guid budgetId, Guid ownerId)) = command;
        BudgetPermission budgetPermission;

        if (budgetPermissionId.HasValue)
        {
            BudgetPermissionId typedBudgetPermissionId = BudgetPermissionId.New(budgetPermissionId.Value);

            budgetPermission =
                await _budgetPermissionRepository.GetByIdAsync(typedBudgetPermissionId, cancellationToken) ??
                BudgetPermission.Create(typedBudgetPermissionId, BudgetId.New(budgetId), PersonId.New(ownerId));
        }
        else
        {
            budgetPermission = BudgetPermission.Create(BudgetId.New(budgetId), PersonId.New(ownerId));
        }

        budgetPermission.AddPermission(PersonId.New(ownerId), PermissionType.Owner);
        await _budgetPermissionRepository.AddOrUpdateAsync(budgetPermission, cancellationToken);

        return new CreateBudgetPermissionResponse(budgetPermission.Id.Value);
    }
}