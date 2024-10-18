using FluentValidation;

namespace Expenso.Shared.Commands.Validation.Validators;

public abstract class CommandValidator<TCommand> : AbstractValidator<TCommand> where TCommand : class, ICommand
{
    protected CommandValidator(MessageContextValidator messageContextValidator)
    {
        RuleFor(expression: x => x.MessageContext).SetValidator(validator: messageContextValidator);
    }
}