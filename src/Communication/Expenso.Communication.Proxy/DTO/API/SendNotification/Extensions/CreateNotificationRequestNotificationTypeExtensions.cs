using Expenso.Communication.Proxy.DTO.Settings;

namespace Expenso.Communication.Proxy.DTO.API.SendNotification.Extensions;

public static class CreateNotificationRequestNotificationTypeExtensions
{
    public static SendNotificationRequest_NotificationType CreateNotificationTypeBasedOnSettings(
        this NotificationSettings? settings)
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

    private static SendNotificationRequest_NotificationType Disable()
    {
        return new SendNotificationRequest_NotificationType(Email: false, Push: false, InApp: false);
    }
}