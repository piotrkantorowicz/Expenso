namespace Expenso.Communication.Proxy.DTO.API.SendNotification;

public sealed record SendNotificationRequest_NotificationType(
    bool? Email = null,
    bool? Push = null,
    bool? InApp = null);