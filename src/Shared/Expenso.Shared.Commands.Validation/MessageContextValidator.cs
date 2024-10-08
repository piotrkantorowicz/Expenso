using Expenso.Shared.System.Types.Messages.Interfaces;

using FluentValidation;

namespace Expenso.Shared.Commands.Validation;

public sealed class MessageContextValidator : AbstractValidator<IMessageContext?>
{
    public MessageContextValidator()
    {
        RuleFor(expression: x => x!.CorrelationId)
            .NotEmpty()
            .WithMessage(errorMessage: "Correlation id cannot be empty.");

        RuleFor(expression: x => x!.MessageId).NotEmpty().WithMessage(errorMessage: "Message id cannot be empty.");
        RuleFor(expression: x => x!.ModuleId).NotEmpty().WithMessage(errorMessage: "Module id cannot be empty.");
        RuleFor(expression: x => x!.Timestamp).NotEmpty().WithMessage(errorMessage: "Timestamp cannot be empty.");
    }
}