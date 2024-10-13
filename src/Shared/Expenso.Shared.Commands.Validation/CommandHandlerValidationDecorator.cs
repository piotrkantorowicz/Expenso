namespace Expenso.Shared.Commands.Validation;

internal sealed class CommandHandlerValidationDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators;

    public CommandHandlerValidationDecorator(IEnumerable<ICommandValidator<TCommand>> validators,
        ICommandHandler<TCommand> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _validators = validators ?? throw new ArgumentNullException(paramName: nameof(validators));
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = _validators
            .Select(selector: x => x.Validate(command: command))
            .SelectMany(selector: x => x)
            .GroupBy(keySelector: x => x.Key)
            .ToDictionary(keySelector: g => g.Key,
                elementSelector: g =>
                    string.Join(separator: Environment.NewLine, values: g.Select(selector: e => e.Value)));

        if (errors.Count is not 0)
        {
            throw new CommandValidationException(errorDictionary: errors);
        }

        await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
    }
}

internal sealed class CommandHandlerValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly IEnumerable<ICommandValidator<TCommand>> _validators;

    public CommandHandlerValidationDecorator(IEnumerable<ICommandValidator<TCommand>> validators,
        ICommandHandler<TCommand, TResult> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _validators = validators ?? throw new ArgumentNullException(paramName: nameof(validators));
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = _validators
            .Select(selector: x => x.Validate(command: command))
            .SelectMany(selector: x => x)
            .GroupBy(keySelector: x => x.Key)
            .ToDictionary(keySelector: g => g.Key,
                elementSelector: g =>
                    string.Join(separator: Environment.NewLine, values: g.Select(selector: e => e.Value)));
        
        if (errors.Count is not 0)
        {
            throw new CommandValidationException(errorDictionary: errors);
        }

        return await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
    }
}