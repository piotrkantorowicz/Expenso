using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class
    BudgetPermissionRequestConfirmedEventHandler : IDomainEventHandler<BudgetPermissionRequestConfirmedEvent>
{
    public async Task HandleAsync(BudgetPermissionRequestConfirmedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRequestCancelledEventHandler
        await Task.CompletedTask;
    }
}