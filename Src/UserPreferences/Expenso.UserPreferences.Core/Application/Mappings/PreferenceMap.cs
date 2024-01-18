using Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Core.Application.Mappings;

internal static class PreferenceMap
{
    public static PreferenceDto MapToDto(Preference preference)
    {
        return new PreferenceDto(preference.PreferenceId, preference.UserId,
            FinancePreferenceMap.MapToDto(preference.FinancePreference!),
            NotificationPreferenceMap.MapToDto(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToDto(preference.GeneralPreference!));
    }

    public static PreferenceContract MapToContract(Preference preference)
    {
        return new PreferenceContract(preference.PreferenceId, preference.UserId,
            FinancePreferenceMap.MapToContract(preference.FinancePreference!),
            NotificationPreferenceMap.MapToContract(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToContract(preference.GeneralPreference!));
    }
}