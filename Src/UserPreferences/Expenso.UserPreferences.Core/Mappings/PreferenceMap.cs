using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Mappings;

internal static class PreferenceMap
{
    public static PreferenceDto MapToDto(Preference preference)
    {
        return new PreferenceDto(preference.PreferencesId, preference.UserId,
            FinancePreferenceMap.MapToDto(preference.FinancePreference!),
            NotificationPreferenceMap.MapToDto(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToDto(preference.GeneralPreference!));
    }

    public static PreferenceContract MapToContract(Preference preference)
    {
        return new PreferenceContract(preference.PreferencesId, preference.UserId,
            FinancePreferenceMap.MapToContract(preference.FinancePreference!),
            NotificationPreferenceMap.MapToContract(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToContract(preference.GeneralPreference!));
    }
}