using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External.DTO.Maps;

internal static class GetPreferenceResponseMap
{
    public static GetPreferenceResponse MapTo(Preference preference)
    {
        return new GetPreferenceResponse(MapTo(preference.FinancePreference), MapTo(preference.NotificationPreference),
            MapTo(preference.GeneralPreference));
    }

    private static GetFinancePreferenceResponse? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetFinancePreferenceResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetNotificationPreferenceResponse? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetNotificationPreferenceResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    private static GetGeneralPreferenceResponse? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetGeneralPreferenceResponse(generalPreference.UseDarkMode);
    }
}