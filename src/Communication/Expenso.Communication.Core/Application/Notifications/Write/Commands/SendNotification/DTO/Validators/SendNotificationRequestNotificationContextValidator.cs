using Expenso.Communication.Shared.DTO.API.SendNotification;

using FluentValidation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;

internal sealed class
    SendNotificationRequestNotificationContextValidator : AbstractValidator<SendNotificationRequestNotificationContext>
{
    public SendNotificationRequestNotificationContextValidator()
    {
        RuleFor(expression: x => x.To).NotEmpty().WithMessage(errorMessage: "To is required.");
        RuleFor(expression: x => x.From).NotEmpty().WithMessage(errorMessage: "From is required.");
    }
}