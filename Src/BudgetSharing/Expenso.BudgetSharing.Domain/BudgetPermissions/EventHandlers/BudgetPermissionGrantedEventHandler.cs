using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers;

internal sealed class BudgetPermissionGrantedEventHandler : IDomainEventHandler<BudgetPermissionGrantedEvent>
{
    public async Task HandleAsync(BudgetPermissionGrantedEvent @event, CancellationToken cancellationToken = default)
    {
        // TODO: Implement the BudgetPermissionGrantedEventHandler
        await Task.CompletedTask;
    }
}