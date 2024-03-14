using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Transactions;

public static class Extensions
{
    public static IServiceCollection AddCommandsTransactions(this IServiceCollection services)
    {
        services.TryDecorate(typeof(ICommandHandler<>), typeof(CommandHandlerTransactionDecorator<>));
        services.TryDecorate(typeof(ICommandHandler<,>), typeof(CommandHandlerTransactionDecorator<,>));

        return services;
    }
}