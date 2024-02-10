using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Commands.Transactions;

public static class RegistrationExtensions
{
    public static IServiceCollection AddCommandsTransactions(this IServiceCollection services)
    {
        services.Decorate(typeof(ICommandHandler<>), typeof(CommandHandlerTransactionDecorator<>));
        services.Decorate(typeof(ICommandHandler<,>), typeof(CommandHandlerTransactionDecorator<,>));

        return services;
    }
}