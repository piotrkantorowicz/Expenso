using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Communication.Proxy;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionRestoredEventHandler : IDomainEventHandler<BudgetPermissionRestoredEvent>
{
    private readonly ICommunicationProxy _communicationProxy;

    public BudgetPermissionRestoredEventHandler(ICommunicationProxy communicationProxy)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));
    }

    public async Task HandleAsync(BudgetPermissionRestoredEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRestoredEventHandler
        await Task.CompletedTask;
    }
}