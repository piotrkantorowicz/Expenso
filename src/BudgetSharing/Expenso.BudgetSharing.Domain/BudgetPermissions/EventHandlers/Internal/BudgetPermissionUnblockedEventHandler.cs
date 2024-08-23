using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissions.EventHandlers.Internal;

internal sealed class BudgetPermissionUnblockedEventHandler : IDomainEventHandler<BudgetPermissionUnblockedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionUnblockedEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionUnblockedEvent @event, CancellationToken cancellationToken)
    {
        (PersonNotificationModel? owner, IReadOnlyCollection<PersonNotificationModel?> participants) =
            await _iamProxyService.GetUserNotificationAvailability(ownerId: @event.OwnerId,
                participantIds: @event.Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly(),
                cancellationToken: cancellationToken);

        if (owner?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: owner.Person!.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value: "We are writing to inform you that a budget permission has been unblocked for your budget.");

            message.AppendLine(value: "Below are the details of the unblocked permission:");
            message.AppendLine();
            message.Append(value: "- Affected participants: All participants").AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If this unblocking was unintentional or if you require further assistance, please do not hesitate to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Blocked",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email.From, To: owner.Person!.Email),
                NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                    settings: _notificationSettings));

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        foreach (PersonNotificationModel? participant in participants)
        {
            if (participant?.CanSendNotification is true)
            {
                StringBuilder message = new();
                message.Append(value: "Dear Participants,");
                message.AppendLine();

                message.AppendLine(
                    value: "We are pleased to inform you that a budget permission you had has been unblocked.");

                message.AppendLine(value: "Below are the details of the unblocked permission:");
                message.AppendLine();

                if (owner?.Person is not null)
                {
                    message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
                }

                message.AppendLine();

                message.AppendLine(
                    value:
                    "If this unblocking was unintentional or if you require further assistance, please feel free to reach out to us.");

                message.AppendLine();
                message.AppendLine(value: "We appreciate your continued participation.");
                message.AppendLine();
                message.AppendLine(value: "Best regards,");
                message.AppendLine(value: "Expenso Team");

                SendNotificationRequest participantNotification = new(Subject: "Budget Permission Blocked",
                    Content: message.ToString(),
                    NotificationContext: new SendNotificationRequest_NotificationContext(
                        From: _notificationSettings.Email.From, To: participant.Person!.Email),
                    NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                        settings: _notificationSettings));

                await _communicationProxy.SendNotificationAsync(request: participantNotification,
                    cancellationToken: cancellationToken);
            }
        }
    }
}