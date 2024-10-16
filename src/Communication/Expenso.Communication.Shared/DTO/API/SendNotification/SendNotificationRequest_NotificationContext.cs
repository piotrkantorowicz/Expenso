namespace Expenso.Communication.Shared.DTO.API.SendNotification;

public sealed record SendNotificationRequest_NotificationContext(
    string From,
    string To,
    string[]? Cc = null,
    string[]? Bcc = null,
    string? ReplyTo = null);