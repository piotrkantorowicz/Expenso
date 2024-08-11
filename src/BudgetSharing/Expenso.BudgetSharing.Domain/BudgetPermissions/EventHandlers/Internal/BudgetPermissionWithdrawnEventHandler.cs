using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionWithdrawnEventHandler : IDomainEventHandler<BudgetPermissionWithdrawnEvent>
{
    public async Task HandleAsync(BudgetPermissionWithdrawnEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionWithdrawnEventHandler
        await Task.CompletedTask;
    }
}