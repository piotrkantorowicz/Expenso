using Expenso.Shared.Types.Exceptions;

namespace Expenso.Shared.Commands;

internal class CommandHandlerValidationDecorator<TCommand>(
    IEnumerable<ICommandValidator<TCommand>> validators,
    ICommandHandler<TCommand> decorated) : ICommandHandler<TCommand> where TCommand : class, ICommand
{
    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> errors = validators
            .Select(x => x.Validate(command))
            .SelectMany(x => x)
            .ToDictionary(x => x.Key, x => x.Value);

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        await decorated.HandleAsync(command, cancellationToken);
    }
}

internal class CommandHandlerValidationDecorator<TCommand, TResult>(
    IEnumerable<ICommandValidator<TCommand>> validators,
    ICommandHandler<TCommand, TResult> decorated)
    : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand where TResult : class
{
    public async Task<TResult?> HandleAsync(TCommand command, CancellationToken cancellationToken = default)
    {
        Dictionary<string, string> errors = validators
            .Select(x => x.Validate(command))
            .SelectMany(x => x)
            .ToDictionary(x => x.Key, x => x.Value);

        if (errors.Count != 0)
        {
            throw new ValidationException(errors);
        }

        return await decorated.HandleAsync(command, cancellationToken);
    }
}