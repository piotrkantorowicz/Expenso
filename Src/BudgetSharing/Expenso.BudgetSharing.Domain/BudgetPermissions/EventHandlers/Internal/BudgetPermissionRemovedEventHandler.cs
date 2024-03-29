using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionRemovedEventHandler : IDomainEventHandler<BudgetPermissionDeletedEvent>
{
    public async Task HandleAsync(BudgetPermissionDeletedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRemovedEventHandler
        await Task.CompletedTask;
    }
}