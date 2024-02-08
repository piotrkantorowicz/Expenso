using Expenso.Shared.Types.Clock;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Types;

public static class RegistrationExtensions
{
    public static IServiceCollection AddClock(this IServiceCollection services)
    {
        services.AddSingleton<IClock, UtcClock>();

        return services;
    }
}