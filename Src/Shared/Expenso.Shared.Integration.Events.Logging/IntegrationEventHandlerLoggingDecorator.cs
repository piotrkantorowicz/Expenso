using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Integration.Events.Logging;

internal sealed class IntegrationEventHandlerLoggingDecorator<TEvent>(
    ILogger<IntegrationEventHandlerLoggingDecorator<TEvent>> logger,
    IIntegrationEventHandler<TEvent> decorated,
    ISerializer serializer) : IIntegrationEventHandler<TEvent> where TEvent : class, IIntegrationEvent
{
    private readonly IIntegrationEventHandler<TEvent> _decorated =
        decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));

    private readonly ILogger<IntegrationEventHandlerLoggingDecorator<TEvent>> _logger =
        logger ?? throw new ArgumentNullException(paramName: nameof(logger));

    private readonly ISerializer _serializer =
        serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));

    public async Task HandleAsync(TEvent @event, CancellationToken cancellationToken)
    {
        EventId executing = LoggingUtils.IntegrationEventExecuting;
        EventId executed = LoggingUtils.IntegrationEventExecuted;
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