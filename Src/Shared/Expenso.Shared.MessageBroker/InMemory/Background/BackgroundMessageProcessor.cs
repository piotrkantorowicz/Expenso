using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.MessageBroker.InMemory.Background;

internal sealed class BackgroundMessageProcessor(
    IMessageChannel messageChannel,
    IServiceProvider serviceProvider,
    ILogger<BackgroundMessageProcessor> logger) : BackgroundService
{
    private readonly IMessageChannel _messageChannel =
        messageChannel ?? throw new ArgumentNullException(nameof(messageChannel));

    private readonly IServiceProvider _serviceProvider =
        serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));

    private readonly ILogger<BackgroundMessageProcessor> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Running the background message processor");

        await foreach (var message in _messageChannel.Reader.ReadAllAsync(stoppingToken))
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(message.GetType());
                var handlers = scope.ServiceProvider.GetServices(handlerType);
                List<Task> handlerTasks = [];

                foreach (var handler in handlers)
                {
                    var handleMethod = handler?.GetType().GetMethod("HandleAsync");
                    var handleTask = handleMethod?.Invoke(handler, [message, stoppingToken]);

                    if (handleTask is Task task)
                    {
                        handlerTasks.Add(task);
                    }
                }

                await Task.WhenAll(handlerTasks);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "{ExceptionMessage}", exception.Message);
            }
        }

        _logger.LogInformation("Finished running the background message processor");
    }
}