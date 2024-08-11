using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Transactions;

public static class Extensions
{
    public static IServiceCollection AddCommandsTransactions(this IServiceCollection services)
    {
        services.TryDecorate(serviceType: typeof(ICommandHandler<>),
            decoratorType: typeof(CommandHandlerTransactionDecorator<>));

        services.TryDecorate(serviceType: typeof(ICommandHandler<,>),
            decoratorType: typeof(CommandHandlerTransactionDecorator<,>));

        return services;
    }
}