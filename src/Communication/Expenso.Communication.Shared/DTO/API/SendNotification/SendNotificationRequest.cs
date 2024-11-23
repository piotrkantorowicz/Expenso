namespace Expenso.Communication.Shared.DTO.API.SendNotification;

public sealed record SendNotificationRequest(
    string? Subject,
    string Content,
    SendNotificationRequestNotificationContext? NotificationContext,
    SendNotificationRequestNotificationType? NotificationType);