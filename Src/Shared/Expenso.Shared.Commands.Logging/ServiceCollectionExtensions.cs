using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Logging;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandsLogging(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerLoggingDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(CommandHandlerLoggingDecorator<,>));

        return services;
    }
}