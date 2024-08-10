using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Integration.Events.Logging;

public static class Extensions
{
    public static IServiceCollection AddIntegrationEventsLogging(this IServiceCollection services)
    {
        services.TryDecorate(serviceType: typeof(IIntegrationEventHandler<>),
            decoratorType: typeof(IntegrationEventHandlerLoggingDecorator<>));

        return services;
    }
}