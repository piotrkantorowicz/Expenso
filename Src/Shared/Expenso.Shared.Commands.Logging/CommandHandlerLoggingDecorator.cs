using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Commands.Logging;

internal sealed class CommandHandlerLoggingDecorator<TCommand>(
    ILogger<CommandHandlerLoggingDecorator<TCommand>> logger,
    ICommandHandler<TCommand> decorated,
    ISerializer serializer) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly ILogger<CommandHandlerLoggingDecorator<TCommand>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        EventId executing = LoggingUtils.CommandExecuting;
        EventId executed = LoggingUtils.CommandExecuted;
        string? commandName = command.GetType().FullName;
        string serializedCommand = _serializer.Serialize(command);

        _logger.LogInformation(executing, "[START] {CommandName}. Params: {SerializedCommand}", commandName,
            serializedCommand);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            await _decorated.HandleAsync(command, cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation(executed, "[END] {CommandName}: {ExecutionTime}[ms]", commandName,
                stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(LoggingUtils.UnexpectedException, ex, "[END] {CommandName}: {ExecutionTime}[ms]",
                commandName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}

internal sealed class CommandHandlerLoggingDecorator<TCommand, TResult>(
    ILogger<CommandHandlerLoggingDecorator<TCommand, TResult>> logger,
    ICommandHandler<TCommand, TResult> decorated,
    ISerializer serializer) : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly ILogger<CommandHandlerLoggingDecorator<TCommand, TResult>> _logger =
        logger ?? throw new ArgumentNullException(nameof(logger));

    private readonly ISerializer _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));

    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        EventId executing = LoggingUtils.CommandExecuting;
        EventId executed = LoggingUtils.CommandExecuted;
        string? commandName = command.GetType().FullName;
        string serializedCommand = _serializer.Serialize(command);

        _logger.LogInformation(executing, "[START] {CommandName}. Params: {SerializedCommand}", commandName,
            serializedCommand);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            TResult? result = await _decorated.HandleAsync(command, cancellationToken);
            stopwatch.Stop();

            _logger.LogInformation(executed, "[END] {CommandName}: {ExecutionTime}[ms]", commandName,
                stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(LoggingUtils.UnexpectedException, ex, "[END] {CommandName}: {ExecutionTime}[ms]",
                commandName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}