using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.BudgetPermissions;
using Expenso.BudgetSharing.Domain.BudgetPermissions.Repositories;
using Expenso.Shared.Database;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class BudgetPermissionRequestConfirmedEventHandler(
    IBudgetPermissionRepository budgetPermissionRepository,
    IUnitOfWork unitOfWork) : IDomainEventHandler<BudgetPermissionRequestConfirmedEvent>
{
    private readonly IBudgetPermissionRepository _budgetPermissionRepository = budgetPermissionRepository ??
                                                                               throw new ArgumentNullException(
                                                                                   nameof(budgetPermissionRepository));

    private readonly IUnitOfWork _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));

    public async Task HandleAsync(BudgetPermissionRequestConfirmedEvent @event,
        CancellationToken cancellationToken = default)
    {
        await _unitOfWork.BeginTransactionAsync(cancellationToken);

        BudgetPermission? budgetPermission =
            await _budgetPermissionRepository.GetByBudgetIdAsync(@event.BudgetId, cancellationToken);

        if (budgetPermission is null)
        {
            throw new NotFoundException($"Budget permission for budget with id {@event.BudgetId} hasn't been found");
        }

        budgetPermission.AddPermission(@event.ParticipantId, @event.PermissionType);
        await _budgetPermissionRepository.UpdateAsync(budgetPermission, cancellationToken);
        await _unitOfWork.CommitTransactionAsync(cancellationToken);
    }
}