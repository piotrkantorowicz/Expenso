using System.Diagnostics;

using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;

namespace Expenso.Shared.Commands.Logging;

internal sealed class CommandHandlerLoggingDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly ILoggerService<CommandHandlerLoggingDecorator<TCommand>> _logger;
    private readonly ISerializer _serializer;

    public CommandHandlerLoggingDecorator(ILoggerService<CommandHandlerLoggingDecorator<TCommand>> logger,
        ICommandHandler<TCommand> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        string? commandName = command.GetType().FullName;
        string serializedCommand = _serializer.Serialize(value: command);

        _logger.LogInfo(eventId: LoggingUtils.CommandExecuting,
            message: "[START] {CommandName}. Params: {SerializedCommand}", messageContext: command.MessageContext,
            commandName, serializedCommand);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInfo(eventId: LoggingUtils.CommandExecuted, message: "[END] {CommandName}: {ExecutionTime}[ms]",
                messageContext: command.MessageContext, commandName, stopwatch.ElapsedMilliseconds);
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedError, message: "[END] {CommandName}: {ExecutionTime}[ms]",
                exception: ex, messageContext: command.MessageContext, commandName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}

internal sealed class CommandHandlerLoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly ILoggerService<CommandHandlerLoggingDecorator<TCommand, TResult>> _logger;
    private readonly ISerializer _serializer;

    public CommandHandlerLoggingDecorator(ILoggerService<CommandHandlerLoggingDecorator<TCommand, TResult>> logger,
        ICommandHandler<TCommand, TResult> decorated, ISerializer serializer)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
        _serializer = serializer ?? throw new ArgumentNullException(paramName: nameof(serializer));
    }

    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        string? commandName = command.GetType().FullName;
        string serializedCommand = _serializer.Serialize(value: command);

        _logger.LogInfo(eventId: LoggingUtils.CommandExecuting,
            message: "[START] {CommandName}. Params: {SerializedCommand}", messageContext: command.MessageContext,
            commandName, serializedCommand);

        Stopwatch stopwatch = new();

        try
        {
            stopwatch.Start();
            TResult? result = await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
            stopwatch.Stop();

            _logger.LogInfo(eventId: LoggingUtils.CommandExecuted, message: "[END] {CommandName}: {ExecutionTime}[ms]",
                messageContext: command.MessageContext, commandName, stopwatch.ElapsedMilliseconds);

            return result;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();

            _logger.LogError(eventId: LoggingUtils.UnexpectedError, message: "[END] {CommandName}: {ExecutionTime}[ms]",
                exception: ex, messageContext: command.MessageContext, commandName, stopwatch.ElapsedMilliseconds);

            throw;
        }
    }
}