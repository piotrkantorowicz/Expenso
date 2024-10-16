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

internal sealed class
    BudgetPermissionRequestConfirmedEventHandler : IDomainEventHandler<BudgetPermissionRequestConfirmedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionRequestConfirmedEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IIamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionRequestConfirmedEvent @event, CancellationToken cancellationToken)
    {
        (PersonNotificationModel? owner, IReadOnlyCollection<PersonNotificationModel> participants) =
            await _iamProxyService.GetUserNotificationAvailability(messageContext: @event.MessageContext,
                ownerId: @event.OwnerId, participantIds:
                [
                    @event.ParticipantId
                ], cancellationToken: cancellationToken);

        PersonNotificationModel? participant =
            participants.FirstOrDefault(predicate: x => x.Person?.UserId == @event.ParticipantId.ToString());

        if (owner?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: owner.Person!.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value:
                "We are pleased to inform you that a budget permission request associated with your budget has been confirmed.");

            message.AppendLine(value: "Below are the details of the request:");
            message.AppendLine();

            if (participant?.Person is not null)
            {
                message
                    .Append(value: "- Budget participant: ")
                    .Append(value: participant.Person?.Fullname)
                    .AppendLine();
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();
            message.Append(value: "- Status: Confirmed").AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If you have any questions or need further assistance, please do not hesitate to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your continued attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Request Confirmed",
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

            message.AppendLine(
                value: "We are pleased to inform you that your budget permission request has been confirmed.");

            message.AppendLine(value: "Below are the details of your request:");
            message.AppendLine();

            if (owner?.Person is not null)
            {
                message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
            }

            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();
            message.Append(value: "- Status: Confirmed").AppendLine();
            message.AppendLine();

            message.AppendLine(
                value: "If you have any questions or need further assistance, please feel free to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your participation and attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Request Confirmed",
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