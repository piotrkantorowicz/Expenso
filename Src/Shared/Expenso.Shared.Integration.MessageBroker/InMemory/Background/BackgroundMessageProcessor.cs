using System.Reflection;

using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Integration.MessageBroker.InMemory.Background;

internal sealed class BackgroundMessageProcessor(
    IMessageChannel messageChannel,
    IServiceProvider serviceProvider,
    ILogger<BackgroundMessageProcessor> logger) : BackgroundService
{
    private readonly ILogger<BackgroundMessageProcessor> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly IMessageChannel _messageChannel =
        messageChannel ?? throw new ArgumentNullException(nameof(messageChannel));

    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Running the background message processor");

        await foreach (IIntegrationEvent? message in _messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                Type handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(message.GetType());
                IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(handlerType);
                List<Func<Task>> handlerTasks = [];

                foreach (object? handler in handlers)
                {
                    MethodInfo? handleAsyncMethod = handler
                        ?.GetType()
                        .GetMethod(nameof(IIntegrationEventHandler<IIntegrationEvent>.HandleAsync));

                    handlerTasks.Add(HandlerFunc);

                    continue;

                    Task HandlerFunc()
                    {
                        if (handleAsyncMethod == null)
                        {
                            return Task.CompletedTask;
                        }

                        object? func = handleAsyncMethod.Invoke(handler, [
                            message,
                            stoppingToken
                        ]);

                        return (Task)func!;
                    }
                }

                await Task.WhenAll(handlerTasks.Select(handlerFunc => handlerFunc()));
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{ExceptionMessage}", exception.Message);
            }
        }

        _logger.LogInformation("Finished running the background message processor");
    }
}