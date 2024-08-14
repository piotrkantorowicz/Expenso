using System.Reflection;

using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Integration.MessageBroker.InMemory.Background;

internal sealed class BackgroundMessageProcessor : BackgroundService
{
    private readonly ILogger<BackgroundMessageProcessor> _logger;
    private readonly IMessageChannel _messageChannel;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundMessageProcessor(IMessageChannel messageChannel, IServiceProvider serviceProvider,
        ILogger<BackgroundMessageProcessor> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _messageChannel = messageChannel ?? throw new ArgumentNullException(paramName: nameof(messageChannel));
        _serviceProvider = serviceProvider ?? throw new ArgumentNullException(paramName: nameof(serviceProvider));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation(message: "Running the background message processor");

        await foreach (IIntegrationEvent? message in _messageChannel.Reader.ReadAllAsync(
                           cancellationToken: stoppingToken))
        {
            try
            {
                using IServiceScope scope = _serviceProvider.CreateScope();
                Type handlerType = typeof(IIntegrationEventHandler<>).MakeGenericType(message.GetType());
                IEnumerable<object?> handlers = scope.ServiceProvider.GetServices(serviceType: handlerType);
                List<Func<Task>> handlerTasks = [];

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
            }
            catch (Exception exception)
            {
                _logger.LogError(exception: exception, message: "{ExceptionMessage}", exception.Message);
            }
        }

        _logger.LogInformation(message: "Finished running the background message processor");
    }
}