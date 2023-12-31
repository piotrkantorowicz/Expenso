using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Mappings;

internal static class NotificationPreferenceMap
{
    public static NotificationPreferenceDto MapToDto(NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceDto(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static NotificationPreferenceContract MapToContract(NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceContract(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static NotificationPreference MapToModel(UpdateNotificationPreferenceDto updateNotificationPreferenceDto)
    {
        return NotificationPreference.Create(updateNotificationPreferenceDto.SendFinanceReportEnabled,
            updateNotificationPreferenceDto.SendFinanceReportInterval);
    }
}