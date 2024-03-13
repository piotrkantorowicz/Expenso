namespace Expenso.Communication.Proxy.DTO.API.SendNotification;

public sealed record SendNotificationRequest(
    string? Subject,
    string Content,
    SendNotificationRequest_NotificationContext? NotificationContext,
    SendNotificationRequest_NotificationType? NotificationType);