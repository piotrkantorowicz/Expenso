using Expenso.Communication.Proxy.DTO.Settings;

namespace Expenso.Communication.Proxy.DTO.API.SendNotification;

public sealed record SendNotificationRequest_NotificationType(bool? Email = null, bool? Push = null, bool? InApp = null)
{
    public static SendNotificationRequest_NotificationType FromSettings(NotificationSettings? settings)
    {
        if (settings is null)
        {
            return Disable();
        }

        if (settings.Enabled is false)
        {
            return Disable();
        }

        return new SendNotificationRequest_NotificationType(Email: settings.Email.Enabled,
            InApp: settings.InApp.Enabled, Push: settings.Push.Enabled);
    }

    public static SendNotificationRequest_NotificationType Disable()
    {
        return new SendNotificationRequest_NotificationType(Email: false, Push: false, InApp: false);
    }
}