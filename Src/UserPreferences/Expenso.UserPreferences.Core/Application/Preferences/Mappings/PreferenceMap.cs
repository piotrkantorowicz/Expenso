using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Mappings;

internal static class PreferenceMap
{
    public static GetPreferenceResponse MapToGetResponse(Preference preference)
    {
        return new GetPreferenceResponse(preference.Id.Value, preference.UserId.Value,
            FinancePreferenceMap.MapToGetResponse(preference.FinancePreference!),
            NotificationPreferenceMap.MapToGetResponse(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToGetResponse(preference.GeneralPreference!));
    }

    public static CreatePreferenceResponse MapToCreateResponse(Preference preference)
    {
        return new CreatePreferenceResponse(preference.Id.Value, preference.UserId.Value,
            FinancePreferenceMap.MapToCreateResponse(preference.FinancePreference!),
            NotificationPreferenceMap.MapToCreateResponse(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToCreateResponse(preference.GeneralPreference!));
    }

    public static GetPreferenceInternalResponse MapToInternalGetRequest(Preference preference)
    {
        return new GetPreferenceInternalResponse(
            FinancePreferenceMap.MapToInternalGetRequest(preference.FinancePreference!),
            NotificationPreferenceMap.MapToInternalGetRequest(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToInternalGetRequest(preference.GeneralPreference!));
    }

    public static CreatePreferenceInternalResponse MapToInternalCreateResponse(Preference preference)
    {
        return new CreatePreferenceInternalResponse(preference.Id.Value, preference.UserId.Value,
            FinancePreferenceMap.MapToInternalCreateResponse(preference.FinancePreference!),
            NotificationPreferenceMap.MapToInternalCreateResponse(preference.NotificationPreference!),
            GeneralPreferenceMap.MapToInternalCreateResponse(preference.GeneralPreference!));
    }
}