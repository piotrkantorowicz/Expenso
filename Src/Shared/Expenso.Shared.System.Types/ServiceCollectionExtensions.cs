using Expenso.Shared.System.Types.Clock;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Types;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClock(this IServiceCollection services)
    {
        services.AddSingleton<IClock, UtcClock>();

        return services;
    }
}