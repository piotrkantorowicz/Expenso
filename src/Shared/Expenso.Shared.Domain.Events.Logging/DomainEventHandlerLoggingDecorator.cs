using System.Diagnostics;

using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

namespace Expenso.Shared.Domain.Events.Logging;

internal sealed class DomainEventHandlerLoggingDecorator<TEvent> : IDomainEventHandler<TEvent>
    where TEvent : class, IDomainEvent
{
    private readonly IDomainEventHandler<TEvent> _decorated;
    private readonly ILoggerService<DomainEventHandlerLoggingDecorator<TEvent>> _logger;
    private readonly ISerializer _serializer;

    public DomainEventHandlerLoggingDecorator(ILoggerService<DomainEventHandlerLoggingDecorator<TEvent>> logger,
        IDomainEventHandler<TEvent> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
    {
        string? eventName = @event.GetType().FullName;
        string serializedEvent = _serializer.Serialize(value: @event);

        _logger.LogInfo(eventId: LoggingUtils.DomainEventExecuting,
            message: "[START] {EventName}. Params: {SerializedEvent}", messageContext: @event.MessageContext, eventName,
            serializedEvent);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            await _decorated.HandleAsync(@event: @event, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInfo(eventId: LoggingUtils.DomainEventExecuted,
                message: "[END] {EventName}: {ExecutionTime}[ms]", messageContext: @event.MessageContext, eventName,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedError, message: "[END] {EventName}: {ExecutionTime}[ms]",
                exception: ex, messageContext: @event.MessageContext, eventName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}