﻿using Expenso.Shared.Commands;
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
        [key: nameof(Commands)] = typeof(ICommand).Assembly,
        [key: nameof(CommandsLogging)] = typeof(CommandHandlerLoggingDecorator<>).Assembly,
        [key: nameof(CommandsTransactions)] = typeof(CommandHandlerTransactionDecorator<>).Assembly,
        [key: nameof(CommandsValidations)] = typeof(CommandHandlerValidationDecorator<>).Assembly
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}