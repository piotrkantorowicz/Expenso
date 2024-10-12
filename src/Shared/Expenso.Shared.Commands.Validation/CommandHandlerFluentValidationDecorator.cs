using System.Text;

using FluentValidation;

namespace Expenso.Shared.Commands.Validation;

internal sealed class CommandHandlerFluentValidationDecorator<TCommand> : ICommandHandler<TCommand>
    where TCommand : class, ICommand
{
    private readonly ICommandHandler<TCommand> _decorated;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public CommandHandlerFluentValidationDecorator(IEnumerable<IValidator<TCommand>> validators,
        ICommandHandler<TCommand> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _validators = validators ?? throw new ArgumentNullException(paramName: nameof(validators));
    }

    public async Task HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = _validators
            .Select(selector: x => x.Validate(instance: command))
            .SelectMany(selector: x => x.Errors)
            .GroupBy(keySelector: x => x.PropertyName)
            .ToDictionary(keySelector: g => g.Key, elementSelector: g =>
            {
                StringBuilder sb = new();

                foreach (string? error in g.Select(selector: e => e.ErrorMessage))
                {
                    sb.AppendLine(value: error);
                }

                return sb.ToString().TrimEnd();
            });

        if (errors.Count is not 0)
        {
            throw new CommandValidationException(errorDictionary: errors);
        }

        await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
    }
}

internal sealed class CommandHandlerFluentValidationDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult>
    where TCommand : class, ICommand where TResult : class
{
    private readonly ICommandHandler<TCommand, TResult> _decorated;
    private readonly IEnumerable<IValidator<TCommand>> _validators;

    public CommandHandlerFluentValidationDecorator(IEnumerable<IValidator<TCommand>> validators,
        ICommandHandler<TCommand, TResult> decorated)
    {
        _decorated = decorated ?? throw new ArgumentNullException(paramName: nameof(decorated));
        _validators = validators ?? throw new ArgumentNullException(paramName: nameof(validators));
    }

    public async Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken)
    {
        Dictionary<string, string> errors = _validators
            .Select(selector: x => x.Validate(instance: command))
            .SelectMany(selector: x => x.Errors)
            .GroupBy(keySelector: x => x.PropertyName)
            .ToDictionary(keySelector: g => g.Key, elementSelector: g =>
            {
                StringBuilder sb = new();

                foreach (string? error in g.Select(selector: e => e.ErrorMessage))
                {
                    sb.AppendLine(value: error);
                }

                return sb.ToString().TrimEnd();
            });

        if (errors.Count is not 0)
        {
            throw new CommandValidationException(errorDictionary: errors);
        }

        return await _decorated.HandleAsync(command: command, cancellationToken: cancellationToken);
    }
}