using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications;
using Expenso.BudgetSharing.Domain.Shared.Shared.Notifications.Models;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Shared.Domain.Events;

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
                value:
                "We are writing to inform you that a budget permission request has been submitted for your budget.");

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
            message.AppendLine(value: "We have received your budget permission request.");
            message.AppendLine(value: "Below are the details of your request:");
            message.AppendLine();

            if (owner?.Person is not null)
            {
                message.Append(value: "- Budget Owner: ").Append(value: owner.Person?.Fullname).AppendLine();
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
                    From: _notificationSettings.Email.From, To: participant.Person!.Email),
                NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                    settings: _notificationSettings));

            await _communicationProxy.SendNotificationAsync(request: participantNotification,
                cancellationToken: cancellationToken);
        }
    }
}