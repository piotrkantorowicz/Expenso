using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class BudgetPermissionRequestedEventHandler : IDomainEventHandler<BudgetPermissionRequestedEvent>
{
    public async Task HandleAsync(BudgetPermissionRequestedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRequestedEventHandler
        await Task.CompletedTask;
    }
}