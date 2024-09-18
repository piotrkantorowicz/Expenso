using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissions.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Communication.Proxy.DTO.API.SendNotification.Extensions;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Communication.Proxy.DTO.Settings.Email;
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
        (PersonNotificationModel? owner, IReadOnlyCollection<PersonNotificationModel> participants) =
            await _iamProxyService.GetUserNotificationAvailability(messageContext: @event.MessageContext,
                ownerId: @event.OwnerId, participantIds: new[]
                {
                    @event.ParticipantId
                }, cancellationToken: cancellationToken);

        PersonNotificationModel? participant =
            participants.FirstOrDefault(predicate: x => x.Person?.UserId == @event.ParticipantId.ToString());

        if (owner?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: owner.Person!.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value: "We are writing to inform you that a budget permission has been withdrawn from your budget.");

            message.AppendLine(value: "Below are the details of the withdrawn permission:");
            message.AppendLine();

            if (participant?.Person is not null)
            {
                message
                    .Append(value: "- Budget participant: ")
                    .Append(value: participant.Person?.Fullname)
                    .AppendLine();
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
                    To: owner.Person!.Email),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        if (participant?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: participant.Person?.Fullname).Append(value: ',');
            message.AppendLine();
            message.AppendLine(value: "We regret to inform you that your budget permission has been withdrawn.");
            message.AppendLine(value: "Below are the details of the withdrawn permission:");
            message.AppendLine();

            if (owner?.Person is not null)
            {
                message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
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

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Granted",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: participant.Person!.Email),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: participantNotification,
                cancellationToken: cancellationToken);
        }
    }
}