using Expenso.Shared.Commands.Validation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandValidator : ICommandValidator<SendNotificationCommand>
{
    public IDictionary<string, string> Validate(SendNotificationCommand command)
    {
        var errors = new Dictionary<string, string>();

        if (command.SendNotificationRequest is null)
        {
            errors.Add(nameof(command.SendNotificationRequest), "Send notification request is required");

            return errors;
        }

        if (command.SendNotificationRequest?.NotificationContext is null)
        {
            errors.Add(nameof(command.SendNotificationRequest.NotificationContext), "Notification context is required");

            return errors;
        }

        if (command.SendNotificationRequest?.NotificationType is null)
        {
            errors.Add(nameof(command.SendNotificationRequest.NotificationType), "Notification type is required");

            return errors;
        }

        if (string.IsNullOrEmpty(command.SendNotificationRequest?.Content) ||
            command.SendNotificationRequest?.Content.Length > 2500)
        {
            errors.Add(nameof(command.SendNotificationRequest.Content),
                "Content is required and must be less than 2500 characters");
        }

        if (string.IsNullOrEmpty(command.SendNotificationRequest?.NotificationContext?.To))
        {
            errors.Add(nameof(command.SendNotificationRequest.NotificationContext.To), "To is required");
        }

        if (string.IsNullOrEmpty(command.SendNotificationRequest?.NotificationContext?.From))
        {
            errors.Add(nameof(command.SendNotificationRequest.NotificationContext.From), "From is required");
        }

        if (command.SendNotificationRequest?.NotificationType is { Push: false, Email: false, InApp: false })
        {
            errors.Add(nameof(command.SendNotificationRequest.NotificationType),
                "At least one notification type is required");
        }

        return errors;
    }
}