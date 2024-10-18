using Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;
using Expenso.Shared.Commands.Validation;

using FluentValidation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandValidator : CommandValidator<SendNotificationCommand>
{
    public SendNotificationCommandValidator(MessageContextValidator messageContextValidator,
        SendNotificationRequestValidator sendNotificationRequestValidator) : base(
        messageContextValidator: messageContextValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: nameof(sendNotificationRequestValidator));

        RuleFor(expression: x => x.Payload!)
            .NotNull()
            .WithMessage(errorMessage: "Send notification request is required.")
            .SetValidator(validator: sendNotificationRequestValidator);
    }
}