using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;

internal static class GetPreferenceResponseMap
{
    public static GetPreferenceResponse MapTo(Preference preference)
    {
        return new GetPreferenceResponse(preference.Id, preference.UserId, MapTo(preference.FinancePreference),
            MapTo(preference.NotificationPreference), MapTo(preference.GeneralPreference));
    }

    private static GetPreferenceResponse_FinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_FinancePreference(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferenceResponse_NotificationPreference? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_NotificationPreference(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferenceResponse_GeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_GeneralPreference(generalPreference.UseDarkMode);
    }
}