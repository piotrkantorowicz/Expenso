using System.Reflection;

using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;
using Expenso.Shared.System.Logging;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Expenso.Shared.Integration.MessageBroker.InMemory.Background;

internal sealed class BackgroundMessageProcessor : BackgroundService
{
    private readonly ILoggerService<BackgroundMessageProcessor> _logger;
    private readonly IMessageChannel _messageChannel;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundMessageProcessor(IMessageChannel messageChannel, IServiceProvider serviceProvider,
        ILoggerService<BackgroundMessageProcessor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _messageChannel = messageChannel ?? throw new ArgumentNullException(paramName: nameof(messageChannel));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.BackgroundJobStarting,
            message: "Running the background message processor");

        return base.StartAsync(cancellationToken: cancellationToken);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInfo(eventId: LoggingUtils.BackgroundJobFinished,
            message: "Finished running the background message processor");

        return base.StopAsync(cancellationToken: cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogDebug(eventId: LoggingUtils.BackgroundJobGeneralInformation,
            message: "Start processing entry for background message processor...");

        await foreach (IIntegrationEvent? message in _messageChannel.Reader.ReadAllAsync(
                           cancellationToken: stoppingToken))
        {
            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                Type handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(message.GetType());
                object?[] handlers = scope.ServiceProvider.GetServices(serviceType: handlerType).ToArray();
                List<Func<Task>> handlerTasks = [];

                _logger.LogDebug(eventId: LoggingUtils.BackgroundJobGeneralInformation,
                    message: "{HandlersCount} handlers have been found for message: {Message}",
                    args: [handlers.Length, message.GetType()]);

                foreach (object? handler in handlers)
                {
                    MethodInfo? handleAsyncMethod = handler
                        ?.GetType()
                        .GetMethod(name: nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync));

                    handlerTasks.Add(item: HandlerFunc);

                    continue;

                    Task HandlerFunc()
                    {
                        if (handleAsyncMethod == null)
                        {
                            return Task.CompletedTask;
                        }

                        object? func = handleAsyncMethod.Invoke(obj: handler, parameters:
                        [
                            message,
                            stoppingToken
                        ]);

                        return (Task)func!;
                    }
                }

                await Task.WhenAll(tasks: handlerTasks.Select(selector: handlerFunc => handlerFunc()));

                _logger.LogDebug(eventId: LoggingUtils.BackgroundJobGeneralInformation, message: "Published {Message}",
                    args: [message.GetType()]);
            }
            catch (Exception exception)
            {
                _logger.LogError(eventId: LoggingUtils.BackgroundJobError, exception: exception,
                    message: "{ExceptionMessage}", args: exception.Message);
            }
        }
    }
}