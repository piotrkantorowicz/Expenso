using Expenso.Communication.Shared.DTO.API.SendNotification;

using FluentValidation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;

internal sealed class
    SendNotificationRequest_NotificationTypeValidator : AbstractValidator<SendNotificationRequest_NotificationType>
{
    public SendNotificationRequest_NotificationTypeValidator()
    {
        RuleFor(expression: x => x)
            .Must(predicate: x => x.Push is true || x.Email is true || x.InApp is true)
            .WithMessage(errorMessage: "At least one notification type is required.");
    }
}