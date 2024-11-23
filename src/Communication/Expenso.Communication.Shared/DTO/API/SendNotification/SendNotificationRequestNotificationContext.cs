namespace Expenso.Communication.Shared.DTO.API.SendNotification;

public sealed record SendNotificationRequestNotificationContext(
    string From,
    string To,
    string[]? Cc = null,
    string[]? Bcc = null,
    string? ReplyTo = null);