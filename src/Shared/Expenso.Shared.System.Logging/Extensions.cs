using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Logging;

public static class Extensions
{
    public static IServiceCollection AddInternalLogging(this IServiceCollection services)
    {
        services.AddSingleton(serviceType: typeof(ILoggerService<>), implementationType: typeof(LoggerService<>));

        return services;
    }
}