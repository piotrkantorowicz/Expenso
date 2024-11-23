using Expenso.Communication.Shared.DTO.Settings;

namespace Expenso.Communication.Shared.DTO.API.SendNotification.Extensions;

public static class CreateNotificationRequestNotificationTypeExtensions
{
    public static SendNotificationRequestNotificationType CreateNotificationTypeBasedOnSettings(
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

        return new SendNotificationRequestNotificationType(Email: settings.Email?.Enabled,
            InApp: settings.InApp?.Enabled, Push: settings.Push?.Enabled);
    }

    private static SendNotificationRequestNotificationType Disable()
    {
        return new SendNotificationRequestNotificationType(Email: false, Push: false, InApp: false);
    }
}