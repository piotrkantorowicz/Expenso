using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Logging;

public static class Extensions
{
    public static IServiceCollection AddCommandsLogging(this IServiceCollection services)
    {
        services.TryDecorate(serviceType: typeof(ICommandHandler<>),
            decoratorType: typeof(CommandHandlerLoggingDecorator<>));

        services.TryDecorate(serviceType: typeof(ICommandHandler<,>),
            decoratorType: typeof(CommandHandlerLoggingDecorator<,>));

        return services;
    }
}