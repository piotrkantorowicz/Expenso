using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Integration.Events.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIntegrationEventsLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IIntegrationEventHandler<>), typeof(IntegrationEventHandlerLoggingDecorator<>));

        return services;
    }
}