namespace Expenso.Communication.Shared.DTO.API.SendNotification;

public sealed record SendNotificationRequestNotificationType(
    bool? Email = null,
    bool? Push = null,
    bool? InApp = null);