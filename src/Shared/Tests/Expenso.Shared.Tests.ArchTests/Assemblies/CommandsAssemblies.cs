using Expenso.Shared.Commands;
using Expenso.Shared.Commands.Logging;
using Expenso.Shared.Commands.Transactions;
using Expenso.Shared.Commands.Validation;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class CommandsAssemblies
{
    private static readonly Assembly Commands = typeof(ICommand).Assembly;
    private static readonly Assembly CommandsLogging = typeof(CommandHandlerLoggingDecorator<>).Assembly;
    private static readonly Assembly CommandsTransactions = typeof(CommandHandlerTransactionDecorator<>).Assembly;
    private static readonly Assembly CommandsValidations = typeof(CommandHandlerValidationDecorator<>).Assembly;

    public static IReadOnlyCollection<Assembly> GetAssemblies()
    {
        List<Assembly> assemblies =
        [
            Commands,
            CommandsLogging,
            CommandsTransactions,
            CommandsValidations
        ];

        return assemblies;
    }
}