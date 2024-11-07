using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Shared;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification.Extensions;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class BudgetPermissionRequestedEventHandler : IDomainEventHandler<BudgetPermissionRequestedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionRequestedEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IIamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionRequestedEvent @event, CancellationToken cancellationToken)
    {
        NotificationRecipients notificationRecipients =
            await _iamProxyService.GetUserNotificationAvailability(messageContext: @event.MessageContext,
                ownerId: @event.OwnerId, participantIds:
                [
                    @event.ParticipantId
                ], cancellationToken: cancellationToken);

        NotificationRecipient participant =
            notificationRecipients.Participants.FirstOrDefault() ?? NotificationRecipient.Empty;

        if (notificationRecipients.Owner?.CanSendNotifications is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: notificationRecipients.Owner.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value:
                "We are writing to inform you that a budget permission request has been submitted for your budget.");

            message.AppendLine(value: "Below are the details of the request:");
            message.AppendLine();

            if (participant.CanBeIncludedInNotifications)
            {
                message.AppendLine(handler: $"- Budget participant: {participant.Fullname}");
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();

            message
                .Append(value: "- Submission date: ")
                .Append(value: @event.SubmissionDate.Value.ToString(format: "MMMM dd, yyyy"))
                .AppendLine();

            message.AppendLine();

            message.AppendLine(
                value:
                "Please review this request at your earliest convenience. If you have any questions, feel free to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Requested",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: notificationRecipients.Owner.Email!),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        if (participant.CanSendNotifications)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: participant.Fullname).Append(value: ',');
            message.AppendLine();
            message.AppendLine(value: "We have received your budget permission request.");
            message.AppendLine(value: "Below are the details of your request:");
            message.AppendLine();

            if (notificationRecipients.Owner?.CanBeIncludedInNotifications is true)
            {
                message.AppendLine(handler: $"- Budget Owner: {notificationRecipients.Owner.Fullname}");
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();

            message
                .Append(value: "- Submission date: ")
                .Append(value: @event.SubmissionDate.Value.ToString(format: "MMMM dd, yyyy"))
                .AppendLine();

            message.AppendLine();

            message.AppendLine(
                value: "Your request is currently under review. We will notify you once a decision has been made.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your submission.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Requested",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: participant.Email!),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: participantNotification,
                cancellationToken: cancellationToken);
        }
    }
}