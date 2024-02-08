using Expenso.Shared.Domain.Types.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Domain.Events.Dispatchers;

internal sealed class DomainEventBroker(IServiceProvider serviceProvider) : IDomainEventBroker
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
        where TEvent : class, IDomainEvent
    {
        using IServiceScope scope = _serviceProvider.CreateScope();
        IDomainEventHandler<TEvent> handler = scope.ServiceProvider.GetRequiredService<IDomainEventHandler<TEvent>>();
        await handler.HandleAsync(@event, cancellationToken);
    }

    public async Task PublishMultipleAsync(IEnumerable<IDomainEvent> events,
        CancellationToken cancellationToken = default)
    {
        IEnumerable<Task> handlerTasks = events.Select(@event => PublishAsync(@event, cancellationToken));
        await Task.WhenAll(handlerTasks);
    }
}