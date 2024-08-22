using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.Communication.Proxy;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionGrantedEventHandler : IDomainEventHandler<BudgetPermissionGrantedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;

    public BudgetPermissionGrantedEventHandler(ICommunicationProxy communicationProxy)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));
    }

    public async Task HandleAsync(BudgetPermissionGrantedEvent @event, CancellationToken cancellationToken)
    {
        // TODO: Implement the BudgetPermissionGrantedEventHandler
        await Task.CompletedTask;
    }
}