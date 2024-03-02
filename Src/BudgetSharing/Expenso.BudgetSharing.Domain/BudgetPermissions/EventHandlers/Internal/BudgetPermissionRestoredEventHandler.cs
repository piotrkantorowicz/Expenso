using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionRestoredEventHandler : IDomainEventHandler<BudgetPermissionRestoredEvent>
{
    public async Task HandleAsync(BudgetPermissionRestoredEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRestoredEventHandler
        await Task.CompletedTask;
    }
}