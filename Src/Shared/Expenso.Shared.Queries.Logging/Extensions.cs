using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries.Logging;

public static class Extensions
{
    public static IServiceCollection AddQueryLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(IQueryHandler<,>), typeof(QueryHandlerLoggingDecorator<,>));

        return services;
    }
}