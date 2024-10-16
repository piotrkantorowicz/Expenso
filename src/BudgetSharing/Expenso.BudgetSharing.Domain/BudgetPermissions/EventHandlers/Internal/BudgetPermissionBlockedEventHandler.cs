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

internal sealed class BudgetPermissionBlockedEventHandler : IDomainEventHandler<BudgetPermissionBlockedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxyService _iamProxyService;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionBlockedEventHandler(ICommunicationProxy communicationProxy,
        NotificationSettings notificationSettings, IIamProxyService iamProxyService)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));

        _iamProxyService = iamProxyService ?? throw new ArgumentNullException(paramName: nameof(iamProxyService));
    }

    public async Task HandleAsync(BudgetPermissionBlockedEvent @event, CancellationToken cancellationToken)
    {
        (PersonNotificationModel? owner, IReadOnlyCollection<PersonNotificationModel?> participants) =
            await _iamProxyService.GetUserNotificationAvailability(messageContext: @event.MessageContext,
                ownerId: @event.OwnerId,
                participantIds: @event.Permissions.Select(selector: x => x.ParticipantId).ToList().AsReadOnly(),
                cancellationToken: cancellationToken);

        IReadOnlyCollection<PersonNotificationModel?> existingParticipants = participants
            .Where(predicate: x =>
                @event.Permissions.Select(selector: y => y.ParticipantId.ToString()).Contains(value: x?.Person?.UserId))
            .ToList()
            .AsReadOnly();

        if (owner?.CanSendNotification is true)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: owner.Person!.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value: "We are writing to inform you that a budget permission has been blocked for your budget.");

            message.AppendLine(value: "Below are the details of the deleted permission:");
            message.AppendLine();
            message.Append(value: "- Affected participants: All participants").AppendLine();

            message
                .Append(value: "- Blocked date: ")
                .Append(value: @event.BlockDate?.Value.ToString(format: "MMMM dd, yyyy"))
                .AppendLine();

            message.AppendLine();

            message.AppendLine(
                value:
                "If this blocked was unintentional or if you require further assistance, please do not hesitate to reach out to us.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Blocked",
                Content: message.ToString(),
                NotificationContext: new SendNotificationRequest_NotificationContext(
                    From: _notificationSettings.Email?.From ??
                          throw new ConfigurationValueMissedException(key: nameof(EmailNotificationSettings.From)),
                    To: owner.Person!.Email),
                NotificationType: _notificationSettings.CreateNotificationTypeBasedOnSettings());

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        foreach (PersonNotificationModel? participant in existingParticipants)
        {
            if (participant?.CanSendNotification is true)
            {
                StringBuilder message = new();
                message.Append(value: "Dear ").Append(value: participant.Person?.Fullname).Append(value: ',');
                message.AppendLine();
                message.AppendLine(value: "We regret to inform you that a budget permission you had has been blocked.");
                message.AppendLine(value: "Below are the details of the blocked permission:");
                message.AppendLine();

                if (owner?.Person is not null)
                {
                    message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
                }

                message
                    .Append(value: "- Deletion date: ")
                    .Append(value: @event.BlockDate?.Value.ToString(format: "MMMM dd, yyyy"))
                    .AppendLine();

                message.AppendLine();

                message.AppendLine(
                    value:
                    "If this blocked was unintentional or if you require further assistance, please feel free to reach out to us.");

                message.AppendLine();
                message.AppendLine(value: "We apologize for any inconvenience this may have caused.");
                message.AppendLine();
                message.AppendLine(value: "Best regards,");
                message.AppendLine(value: "Expenso Team");

                SendNotificationRequest participantNotification = new(Subject: "Budget Permission Blocked",
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
}