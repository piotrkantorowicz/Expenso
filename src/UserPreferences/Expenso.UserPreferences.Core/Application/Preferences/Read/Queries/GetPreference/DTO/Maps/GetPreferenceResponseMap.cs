using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Responses;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;

internal static class GetPreferenceResponseMap
{
    public static GetPreferenceResponse MapTo(Preference preference)
    {
        return new GetPreferenceResponse(Id: preference.Id, UserId: preference.UserId,
            FinancePreference: MapTo(financePreference: preference.FinancePreference),
            NotificationPreference: MapTo(notificationPreference: preference.NotificationPreference),
            GeneralPreference: MapTo(generalPreference: preference.GeneralPreference));
    }

    private static GetPreferenceResponseFinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferenceResponseFinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferenceResponseNotificationPreference? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponseNotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferenceResponseGeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponseGeneralPreference(UseDarkMode: generalPreference.UseDarkMode);
    }
}