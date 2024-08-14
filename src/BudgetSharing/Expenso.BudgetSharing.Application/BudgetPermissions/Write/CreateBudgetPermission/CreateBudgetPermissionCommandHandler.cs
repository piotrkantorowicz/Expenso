using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
using Expenso.BudgetSharing.Proxy.DTO.API.CreateBudgetPermission.Response;
using Expenso.Shared.Commands;

namespace Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission;

internal sealed class
    CreateBudgetPermissionCommandHandler : ICommandHandler<CreateBudgetPermissionCommand,
    CreateBudgetPermissionResponse>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository;

    public CreateBudgetPermissionCommandHandler(IBudgetPermissionRepository budgetPermissionRepository)
    {
        _budgetPermissionRepository = budgetPermissionRepository ??
                                      throw new ArgumentNullException(paramName: nameof(budgetPermissionRepository));
    }

    public async Task<CreateBudgetPermissionResponse?> HandleAsync(CreateBudgetPermissionCommand command,
        CancellationToken cancellationToken)
    {
        (_, (Guid? budgetPermissionId, Guid budgetId, Guid ownerId)) = command;
        BudgetPermission budgetPermission;

        if (budgetPermissionId.HasValue)
        {
            BudgetPermissionId typedBudgetPermissionId = BudgetPermissionId.New(value: budgetPermissionId.Value);

            budgetPermission =
                await _budgetPermissionRepository.GetByIdAsync(id: typedBudgetPermissionId,
                    cancellationToken: cancellationToken) ?? BudgetPermission.Create(
                    budgetPermissionId: typedBudgetPermissionId, budgetId: BudgetId.New(value: budgetId),
                    ownerId: PersonId.New(value: ownerId));
        }
        else
        {
            budgetPermission = BudgetPermission.Create(budgetId: BudgetId.New(value: budgetId),
                ownerId: PersonId.New(value: ownerId));
        }

        budgetPermission.AddPermission(participantId: PersonId.New(value: ownerId),
            permissionType: PermissionType.Owner);

        await _budgetPermissionRepository.AddOrUpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);

        return new CreateBudgetPermissionResponse(BudgetPermissionId: budgetPermission.Id.Value);
    }
}