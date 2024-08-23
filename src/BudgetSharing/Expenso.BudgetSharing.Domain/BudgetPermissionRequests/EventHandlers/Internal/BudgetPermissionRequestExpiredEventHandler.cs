using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Shared.Domain.Events;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class
    BudgetPermissionRequestExpiredEventHandler : IDomainEventHandler<BudgetPermissionRequestExpiredEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionRequestExpiredEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionRequestExpiredEvent @event, CancellationToken cancellationToken)
    {
        (PersonNotificationModel? owner, IReadOnlyCollection<PersonNotificationModel> participants) =
            await _iamProxyService.GetUserNotificationAvailability(ownerId: @event.OwnerId, participantIds: new[]
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
                value:
                "We are writing to inform you that a budget permission request associated with your budget has expired.");

            message.AppendLine(value: "Below are the details of the expired request:");
            message.AppendLine();

            if (participant?.Person is not null)
            {
                message
                    .Append(value: "- Budget participant: ")
                    .Append(value: participant.Person?.Fullname)
                    .AppendLine();
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();

            message
                .Append(value: "- Expiration date: ")
                .Append(value: @event.ExpirationDate?.Value.ToString(format: "MMMM dd, yyyy"))
                .AppendLine();

            message.AppendLine();

            message.AppendLine(
                value:
                "If you believe this request should still be processed or if you need further assistance, please reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Request Expired",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email.From, To: owner.Person!.Email),
                NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                    settings: _notificationSettings));

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        if (participant?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: participant.Person?.Fullname).Append(value: ',');
            message.AppendLine();
            message.AppendLine(value: "We regret to inform you that your budget permission request has expired.");
            message.AppendLine(value: "Below are the details of the expired request:");
            message.AppendLine();

            if (owner?.Person is not null)
            {
                message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();

            message
                .Append(value: "- Expiration date: ")
                .Append(value: @event.ExpirationDate?.Value.ToString(format: "MMMM dd, yyyy"))
                .AppendLine();

            message.AppendLine();

            message.AppendLine(
                value:
                "If you believe this request should still be processed or if you need further assistance, please feel free to reach out to us.");

            message.AppendLine(value: "You may also resubmit the request if necessary.");
            message.AppendLine();
            message.AppendLine(value: "We apologize for any inconvenience this may have caused.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Request Expired",
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