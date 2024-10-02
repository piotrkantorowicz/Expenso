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

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(Commands)] = Commands,
        [key: nameof(CommandsLogging)] = CommandsLogging,
        [key: nameof(CommandsTransactions)] = CommandsTransactions,
        [key: nameof(CommandsValidations)] = CommandsValidations
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}