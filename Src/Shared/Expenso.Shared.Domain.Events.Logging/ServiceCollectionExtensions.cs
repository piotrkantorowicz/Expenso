using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Domain.Events.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomainEventsLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IDomainEventHandler<>), typeof(DomainEventHandlerLoggingDecorator<>));

        return services;
    }
}