using Expenso.Shared.System.Types.Clock;
using Expenso.Shared.System.Types.Messages;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Types;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddClock(this IServiceCollection services)
    {
        services.AddSingleton<IClock, UtcClock>();

        return services;
    }

    public static IServiceCollection AddMessageContext(this IServiceCollection services)
    {
        services.AddSingleton<IMessageContextFactory, MessageContextFactory>();

        return services;
    }
}