using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Shared;
using Expenso.Communication.Shared.DTO.API.SendNotification;
using Expenso.Communication.Shared.DTO.API.SendNotification.Extensions;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Domain.Events;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionWithdrawnEventHandler : IDomainEventHandler<BudgetPermissionWithdrawnEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionWithdrawnEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IIamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionWithdrawnEvent @event, CancellationToken cancellationToken)
    {
        NotificationRecipients notificationRecipients = await _iamProxyService.GetUserNotificationAvailability(
            messageContext: @event.MessageContext, ownerId: @event.OwnerId, participantIds:
            [
                @event.ParticipantId
            ], cancellationToken: cancellationToken);

        NotificationRecipient? participant = notificationRecipients.Participants.FirstOrDefault();

        if (notificationRecipients.Owner?.CanSendNotifications is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: notificationRecipients.Owner.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value: "We are writing to inform you that a budget permission has been withdrawn from your budget.");

            message.AppendLine(value: "Below are the details of the withdrawn permission:");
            message.AppendLine();

            if (participant?.CanBeIncludedInNotifications is true)
            {
                message.Append(value: "- Budget participant: ").AppendLine(value: participant.Fullname);
            }

            message.Append(value: "- Withdrawn permission: ").Append(value: @event.PermissionType).AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If this withdrawal was unintentional or if you require further assistance, please do not hesitate to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Withdrawn",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: notificationRecipients.Owner.Email!),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                messageContext: @event.MessageContext, cancellationToken: cancellationToken);
        }

        if (participant?.CanSendNotifications is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: participant.Fullname).Append(value: ',');
            message.AppendLine();
            message.AppendLine(value: "We regret to inform you that your budget permission has been withdrawn.");
            message.AppendLine(value: "Below are the details of the withdrawn permission:");
            message.AppendLine();

            if (notificationRecipients.Owner?.CanBeIncludedInNotifications is true)
            {
                message.AppendLine(value: $"- Budget Owner: {notificationRecipients.Owner.Fullname}");
            }

            message.Append(value: "- Withdrawn permission: ").Append(value: @event.PermissionType).AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If this withdrawal was unintentional or if you require further assistance, please feel free to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "We apologize for any inconvenience this may have caused.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Withdrawn",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: participant.Email!),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: participantNotification,
                messageContext: @event.MessageContext, cancellationToken: cancellationToken);
        }
    }
}