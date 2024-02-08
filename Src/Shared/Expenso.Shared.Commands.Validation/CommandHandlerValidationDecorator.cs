using Expenso.Shared.Types.Exceptions;

namespace Expenso.Shared.Commands.Validation;

internal class CommandHandlerValidationDecorator<TCommand>(
    IEnumerable<ICommandValidator<TCommand>> validators,
    ICommandHandler<TCommand> decorated) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly IEnumerable<ICommandValidator<TCommand>> _validators =
        validators ?? throw new ArgumentNullException(nameof(validators));

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> errors = _validators
            .Select(x => x.Validate(command))
            .SelectMany(x => x)
            .ToDictionary(x => x.Key, x => x.Value);

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        await _decorated.HandleAsync(command, cancellationToken);
    }
}

internal class CommandHandlerValidationDecorator<TCommand, TResult>(
    IEnumerable<ICommandValidator<TCommand>> validators,
    ICommandHandler<TCommand, TResult> decorated)
    : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated =
        decorated ?? throw new ArgumentNullException(nameof(decorated));

    private readonly IEnumerable<ICommandValidator<TCommand>> _validators =
        validators ?? throw new ArgumentNullException(nameof(validators));

    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> errors = _validators
            .Select(x => x.Validate(command))
            .SelectMany(x => x)
            .ToDictionary(x => x.Key, x => x.Value);

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        return await _decorated.HandleAsync(command, cancellationToken);
    }
}