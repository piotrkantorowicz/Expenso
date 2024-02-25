using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class
    BudgetPermissionRequestCancelledEventHandler : IDomainEventHandler<BudgetPermissionRequestCancelledEvent>
{
    public async Task HandleAsync(BudgetPermissionRequestCancelledEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRequestCancelledEventHandler
        await Task.CompletedTask;
    }
}