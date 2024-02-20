using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class
    BudgetPermissionRequestExpiredEventHandler : IDomainEventHandler<BudgetPermissionRequestCancelledEvent>
{
    public async Task HandleAsync(BudgetPermissionRequestCancelledEvent @event,
        CancellationToken cancellationToken = default)
    {
        // TODO: Implement the BudgetPermissionRequestExpiredEventHandler
        await Task.CompletedTask;
    }
}