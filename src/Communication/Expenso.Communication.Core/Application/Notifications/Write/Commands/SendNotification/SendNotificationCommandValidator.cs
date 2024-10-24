using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;
using Expenso.Shared.Commands.Validation.Validators;

using FluentValidation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandValidator : CommandValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator(MessageContextValidator messageContextValidator,
        SendNotificationRequestValidator sendNotificationRequestValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: sendNotificationRequestValidator);

        RuleFor(expression: x => x.Payload!)
            .NotNull()
            .WithMessage(errorMessage: "Send notification request is required.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.Payload!).SetValidator(validator: sendNotificationRequestValidator));
    }
}