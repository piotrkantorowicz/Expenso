using Expenso.BudgetSharing.Application.BudgetPermissions.Write.CreateBudgetPermission.DTO.Response;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.BudgetSharing.Domain.BudgetPermissions.ValueObjects;
using Expenso.BudgetSharing.Domain.Shared.ValueObjects;
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

    public async Task<CreateBudgetPermissionResponse> HandleAsync(CreateBudgetPermissionCommand command,
        CancellationToken cancellationToken)
    {
        BudgetPermission budgetPermission;

        if (command.Payload?.BudgetPermissionId is { } budgetPermissionId)
        {
            BudgetPermissionId typedBudgetPermissionId = BudgetPermissionId.New(value: budgetPermissionId);

            budgetPermission =
                await _budgetPermissionRepository.GetByIdAsync(id: typedBudgetPermissionId,
                    cancellationToken: cancellationToken) ?? BudgetPermission.Create(
                    budgetPermissionId: typedBudgetPermissionId,
                    budgetId: BudgetId.New(value: command.Payload?.BudgetId),
                    ownerId: PersonId.New(value: command.Payload?.OwnerId));
        }
        else
        {
            budgetPermission = BudgetPermission.Create(budgetId: BudgetId.New(value: command.Payload?.BudgetId),
                ownerId: PersonId.New(value: command.Payload?.OwnerId));
        }

        budgetPermission.AddPermission(participantId: PersonId.New(value: command.Payload?.OwnerId),
            permissionType: PermissionType.Owner);

        await _budgetPermissionRepository.AddOrUpdateAsync(budgetPermission: budgetPermission,
            cancellationToken: cancellationToken);

        return new CreateBudgetPermissionResponse(BudgetPermissionId: budgetPermission.Id.Value);
    }
}