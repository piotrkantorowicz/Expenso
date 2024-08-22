using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Communication.Proxy;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionRemovedEventHandler : IDomainEventHandler<BudgetPermissionDeletedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;

    public BudgetPermissionRemovedEventHandler(ICommunicationProxy communicationProxy)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));
    }

    public async Task HandleAsync(BudgetPermissionDeletedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionRemovedEventHandler
        await Task.CompletedTask;
    }
}