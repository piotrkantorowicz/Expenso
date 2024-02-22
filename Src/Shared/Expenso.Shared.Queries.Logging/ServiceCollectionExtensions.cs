using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddQueryLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(QueryHandlerLoggingDecorator<,>));

        return services;
    }
}