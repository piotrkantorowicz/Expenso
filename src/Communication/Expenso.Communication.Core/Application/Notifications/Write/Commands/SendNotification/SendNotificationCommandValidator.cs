using Expenso.Shared.Commands.Validation;

namespace Expenso.Communication.Core.Application.Notifications.Write.Commands.SendNotification;

internal sealed class SendNotificationCommandValidator : ICommandValidator<SendNotificationCommand>
{
    public IReadOnlyDictionary<string, CommandValidationRule<SendNotificationCommand>[]> GetValidationMetadata()
    {
        throw new NotImplementedException();
    }

    public IDictionary<string, string> Validate(SendNotificationCommand command)
    {
        Dictionary<string, string> errors = new();

        if (command.SendNotificationRequest is null)
        {
            errors.Add(key: nameof(command.SendNotificationRequest), value: "Send notification request is required.");

            return errors;
        }

        if (command.SendNotificationRequest?.NotificationContext is null)
        {
            errors.Add(key: nameof(command.SendNotificationRequest.NotificationContext),
                value: "Notification context is required.");

            return errors;
        }

        if (command.SendNotificationRequest?.NotificationType is null)
        {
            errors.Add(key: nameof(command.SendNotificationRequest.NotificationType),
                value: "Notification type is required.");

            return errors;
        }

        if (string.IsNullOrEmpty(value: command.SendNotificationRequest?.Content) ||
            command.SendNotificationRequest?.Content.Length > 2500)
        {
            errors.Add(key: nameof(command.SendNotificationRequest.Content),
                value: "Content is required and must be less than 2500 characters.");
        }

        if (string.IsNullOrEmpty(value: command.SendNotificationRequest?.NotificationContext?.To))
        {
            errors.Add(key: nameof(command.SendNotificationRequest.NotificationContext.To), value: "To is required.");
        }

        if (string.IsNullOrEmpty(value: command.SendNotificationRequest?.NotificationContext?.From))
        {
            errors.Add(key: nameof(command.SendNotificationRequest.NotificationContext.From),
                value: "From is required.");
        }

        if (command.SendNotificationRequest?.NotificationType is { Push: false, Email: false, InApp: false })
        {
            errors.Add(key: nameof(command.SendNotificationRequest.NotificationType),
                value: "At least one notification type is required.");
        }

        return errors;
    }
}