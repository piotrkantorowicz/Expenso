using System.Reflection;

using Expenso.Shared.Domain.Types.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Domain.Events.Dispatchers;

internal sealed class DomainEventBroker(IServiceProvider serviceProvider) : IDomainEventBroker
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IDomainEvent
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        // Simulates a delay to avoid handling these event as part of the same transaction as incoming command handling.
        // It will be solved in future using a outbox pattern.
        await Task.Delay(1500, cancellationToken);
        await PublishInternal(@event, scope, cancellationToken);
    }

    public async Task PublishMultipleAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        // Simulates a delay to avoid handling these event as part of the same transaction as incoming command handling.
        // It will be solved in future using a outbox pattern.
        await Task.Delay(1500, cancellationToken);

        foreach (IDomainEvent @event in events)
        {
            await PublishInternal(@event, scope, cancellationToken);
        }
    }

    private static async Task PublishInternal(IDomainEvent @event, IServiceScope scope,
        CancellationToken cancellationToken)
    {
        Type? handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
        IEnumerable<object?>? handlers = scope.ServiceProvider.GetServices(handlerType);
        MethodInfo? method = handlerType.GetMethod(nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException($"Event handler for '{@event.GetType().Name}' is invalid.");
        }

        IEnumerable<Task>? tasks = handlers.Select(x => (Task)method.Invoke(x, [
            @event,
            cancellationToken
        ])!);

        await Task.WhenAll(tasks);
    }
}