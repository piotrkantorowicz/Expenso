﻿using Expenso.Communication.Shared.DTO.API.SendNotification;

using FluentValidation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification.DTO.Validators;

internal sealed class SendNotificationRequestValidator : AbstractValidator<SendNotificationRequest>
{
    public SendNotificationRequestValidator(
        SendNotificationRequest_NotificationContextValidator sendNotificationRequestNotificationContextValidator,
        SendNotificationRequest_NotificationTypeValidator sendNotificationRequestNotificationTypeValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: sendNotificationRequestNotificationContextValidator);
        ArgumentNullException.ThrowIfNull(argument: sendNotificationRequestNotificationTypeValidator);

        RuleFor(expression: x => x.NotificationContext!)
            .NotNull()
            .WithMessage(errorMessage: "Notification context is required.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.NotificationContext!)
                    .SetValidator(validator: sendNotificationRequestNotificationContextValidator));

        RuleFor(expression: x => x.NotificationType!)
            .NotNull()
            .WithMessage(errorMessage: "Notification type is required.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.NotificationType!)
                    .SetValidator(validator: sendNotificationRequestNotificationTypeValidator));

        RuleFor(expression: x => x.Content)
            .NotEmpty()
            .WithMessage(errorMessage: "Content is required.")
            .MaximumLength(maximumLength: 2500)
            .WithMessage(errorMessage: "Content must be less than 2500 characters.");
    }
}