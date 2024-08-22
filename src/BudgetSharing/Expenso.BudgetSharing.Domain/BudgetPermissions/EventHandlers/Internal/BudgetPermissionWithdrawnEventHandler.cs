using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Communication.Proxy;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionWithdrawnEventHandler : IDomainEventHandler<BudgetPermissionWithdrawnEvent>
{
    private readonly ICommunicationProxy _communicationProxy;

    public BudgetPermissionWithdrawnEventHandler(ICommunicationProxy communicationProxy)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));
    }

    public async Task HandleAsync(BudgetPermissionWithdrawnEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionWithdrawnEventHandler
        await Task.CompletedTask;
    }
}