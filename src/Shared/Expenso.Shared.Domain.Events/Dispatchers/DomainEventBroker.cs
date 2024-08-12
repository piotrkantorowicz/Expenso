using System.Reflection;

using Expenso.Shared.Domain.Types.Events;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Domain.Events.Dispatchers;

internal sealed class DomainEventBroker(IServiceProvider serviceProvider) : IDomainEventBroker
{
    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
        where TEvent : class, IDomainEvent
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        // Simulates a delay to avoid handling these event as part of the same transaction as incoming command handling.
        // It will be solved in future using a outbox pattern.
        await Task.Delay(millisecondsDelay: 1500, cancellationToken: cancellationToken);
        await PublishInternal(@event: @event, scope: scope, cancellationToken: cancellationToken);
    }

    public async Task PublishMultipleAsync(IEnumerable<IDomainEvent> events, CancellationToken cancellationToken)
    {
        using IServiceScope scope = _serviceProvider.CreateScope();

        // Simulates a delay to avoid handling these event as part of the same transaction as incoming command handling.
        // It will be solved in future using a outbox pattern.
        await Task.Delay(millisecondsDelay: 1500, cancellationToken: cancellationToken);

        foreach (IDomainEvent @event in events)
        {
            await PublishInternal(@event: @event, scope: scope, cancellationToken: cancellationToken);
        }
    }

    private static async Task PublishInternal(IDomainEvent @event, IServiceScope scope,
        CancellationToken cancellationToken)
    {
        Type? handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
        IEnumerable<object?>? handlers = scope.ServiceProvider.GetServices(serviceType: handlerType);
        MethodInfo? method = handlerType.GetMethod(name: nameof(IDomainEventHandler<IDomainEvent>.HandleAsync));

        if (method is null)
        {
            throw new InvalidOperationException(message: $"Event handler for '{@event.GetType().Name}' is invalid");
        }

        IEnumerable<Task>? tasks = handlers.Select(selector: x => (Task)method.Invoke(obj: x, parameters:
        [
            @event,
            cancellationToken
        ])!);

        await Task.WhenAll(tasks: tasks);
    }
}