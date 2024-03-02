using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionGrantedEventHandler : IDomainEventHandler<BudgetPermissionGrantedEvent>
{
    public async Task HandleAsync(BudgetPermissionGrantedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionGrantedEventHandler
        await Task.CompletedTask;
    }
}