using System.Diagnostics;

using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Domain.Events.Logging;

internal sealed class DomainEventHandlerLoggingDecorator<TEvent> : IDomainEventHandler<TEvent>
    where TEvent : class, IDomainEvent
{
    private readonly IDomainEventHandler<TEvent> _decorated;
    private readonly ILogger<DomainEventHandlerLoggingDecorator<TEvent>> _logger;
    private readonly ISerializer _serializer;

    public DomainEventHandlerLoggingDecorator(ILogger<DomainEventHandlerLoggingDecorator<TEvent>> logger,
        IDomainEventHandler<TEvent> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
    {
        EventId executing = LoggingUtils.DomainEventExecuting;
        EventId executed = LoggingUtils.DomainEventExecuted;
        string? eventName = @event.GetType().FullName;
        string serializedEvent = _serializer.Serialize(value: @event);

        _logger.LogInformation(eventId: executing, message: "[START] {EventName}. Params: {SerializedEvent}", eventName,
            serializedEvent);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            await _decorated.HandleAsync(@event: @event, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation(eventId: executed, message: "[END] {EventName}: {ExecutionTime}[ms]", eventName,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedException, exception: ex,
                message: "[END] {EventName}: {ExecutionTime}[ms]", eventName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}