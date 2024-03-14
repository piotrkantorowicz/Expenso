using Expenso.Shared.System.Serialization.Default;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Serialization;

public static class Extensions
{
    public static IServiceCollection AddDefaultSerializer(this IServiceCollection services)
    {
        services.AddSingleton<ISerializer, DefaultSerializer>();

        return services;
    }
}