using System.Text;

using Expenso.BudgetSharing.Domain.BudgetPermissionRequests.Events;
using Expenso.Communication.Proxy;
using Expenso.Communication.Proxy.DTO.API.SendNotification;
using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.IAM.Proxy;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Domain.Events;

using Microsoft.Extensions.Logging;

namespace Expenso.BudgetSharing.Domain.BudgetPermissionRequests.EventHandlers.Internal;

internal sealed class
    BudgetPermissionRequestConfirmedEventHandler : IDomainEventHandler<BudgetPermissionRequestConfirmedEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxy _iamProxy;
    private readonly ILogger<BudgetPermissionRequestConfirmedEventHandler> _logger;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionRequestConfirmedEventHandler(ICommunicationProxy communicationProxy, IIamProxy iamProxy,
        ILogger<BudgetPermissionRequestConfirmedEventHandler> logger, NotificationSettings notificationSettings)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _iamProxy = iamProxy ?? throw new ArgumentNullException(paramName: nameof(iamProxy));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));
    }

    public async Task HandleAsync(BudgetPermissionRequestConfirmedEvent @event, CancellationToken cancellationToken)
    {
        GetUserResponse? owner =
            await _iamProxy.GetUserByIdAsync(userId: @event.OwnerId.ToString(), cancellationToken: cancellationToken);

        GetUserResponse? participant = await _iamProxy.GetUserByIdAsync(userId: @event.ParticipantId.ToString(),
            cancellationToken: cancellationToken);

        bool canSendEmailToOwner = true;
        bool canSendEmailToParticipant = true;

        if (owner is null)
        {
            _logger.LogWarning(
                message: "Cannot send notification to owner '{OwnerId}' as their email could not be found",
                @event.OwnerId);

            canSendEmailToOwner = false;
        }

        if (participant is null)
        {
            _logger.LogWarning(
                message: "Cannot send notification to participant {ParticipantId} as their email could not be found",
                @event.ParticipantId);

            canSendEmailToParticipant = false;
        }

        if (canSendEmailToOwner)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: owner?.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value:
                "We are pleased to inform you that a budget permission request associated with your budget has been confirmed.");

            message.AppendLine(value: "Below are the details of the request:");
            message.AppendLine();
            message.Append(value: "- Budget participant: ").Append(value: participant?.Fullname).AppendLine();
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
                    From: _notificationSettings.Email.From, To: owner!.Email),
                NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                    settings: _notificationSettings));

            await _communicationProxy.SendNotificationAsync(request: ownerNotification,
                cancellationToken: cancellationToken);
        }

        if (canSendEmailToParticipant)
        {
            StringBuilder message = new();
            message.Append(value: "Dear ").Append(value: participant?.Fullname).Append(value: ',');
            message.AppendLine();

            message.AppendLine(
                value: "We are pleased to inform you that your budget permission request has been confirmed.");

            message.AppendLine(value: "Below are the details of your request:");
            message.AppendLine();
            message.Append(value: "- Budget Owner: ").Append(value: owner?.Fullname).AppendLine();
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
                    From: _notificationSettings.Email.From, To: participant!.Email),
                NotificationType: SendNotificationRequest_NotificationType.FromSettings(
                    settings: _notificationSettings));

            await _communicationProxy.SendNotificationAsync(request: participantNotification,
                cancellationToken: cancellationToken);
        }
    }
}