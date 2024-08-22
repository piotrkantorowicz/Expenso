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
    BudgetPermissionRequestCancelledEventHandler : IDomainEventHandler<BudgetPermissionRequestCancelledEvent>
{
    private readonly ICommunicationProxy _communicationProxy;
    private readonly IIamProxy _iamProxy;
    private readonly ILogger<BudgetPermissionRequestCancelledEventHandler> _logger;
    private readonly NotificationSettings _notificationSettings;

    public BudgetPermissionRequestCancelledEventHandler(ICommunicationProxy communicationProxy, IIamProxy iamProxy,
        ILogger<BudgetPermissionRequestCancelledEventHandler> logger, NotificationSettings notificationSettings)
    {
        _communicationProxy =
            communicationProxy ?? throw new ArgumentNullException(paramName: nameof(communicationProxy));

        _iamProxy = iamProxy ?? throw new ArgumentNullException(paramName: nameof(iamProxy));
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));

        _notificationSettings = notificationSettings ??
                                throw new ArgumentNullException(paramName: nameof(notificationSettings));
    }

    public async Task HandleAsync(BudgetPermissionRequestCancelledEvent @event, CancellationToken cancellationToken)
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
            message.AppendLine(value: "We hope this message finds you well.");
            message.AppendLine();

            message.AppendLine(
                value:
                "We are writing to inform you that a budget permission request associated with your budget has been cancelled.");

            message.AppendLine(value: "Below are the details of the request:");
            message.AppendLine();
            message.Append(value: "- Budget participant: ").Append(value: participant?.Fullname).AppendLine();
            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();
            message.Append(value: "- Reason for cancellation: Cancelled by the budget owner").AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If this cancellation was unintentional or if you require further assistance, please do not hesitate to reach out to us.");

            message.AppendLine(
                value:
                "You may ask the participant to resubmit the request or contact our support team for any clarification.");

            message.AppendLine();
            message.AppendLine(value: "Thank you for your attention to this matter.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest ownerNotification = new(Subject: "Budget Permission Request Cancelled",
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
                value: "We regret to inform you that the budget permission request you submitted has been cancelled.");

            message.AppendLine(value: "Below are the details of the request:");
            message.AppendLine();
            message.Append(value: "- Budget Owner: ").Append(value: owner?.Fullname).AppendLine();
            message.Append(value: "- Requested permission: ").Append(value: @event.PermissionType).AppendLine();
            message.Append(value: "- Reason for cancellation: Cancelled by the budget owner").AppendLine();
            message.AppendLine();

            message.AppendLine(
                value:
                "If this cancellation was unintentional or if you require further assistance, please feel free to reach out to us.");

            message.AppendLine(
                value: "You may resubmit the request or contact our support team for any clarification.");

            message.AppendLine();
            message.AppendLine(value: "We apologize for any inconvenience this may have caused.");
            message.AppendLine();
            message.AppendLine(value: "Best regards,");
            message.AppendLine(value: "Expenso Team");

            SendNotificationRequest participantNotification = new(Subject: "Budget Permission Request Cancelled",
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