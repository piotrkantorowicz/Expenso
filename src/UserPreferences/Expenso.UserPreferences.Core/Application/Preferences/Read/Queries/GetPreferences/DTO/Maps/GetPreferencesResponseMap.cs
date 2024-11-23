using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences.DTO.Maps;

internal static class GetPreferencesResponseMap
{
    public static GetPreferencesResponse MapTo(Preference preference)
    {
        return new GetPreferencesResponse(Id: preference.Id, UserId: preference.UserId,
            FinancePreference: MapTo(financePreference: preference.FinancePreference),
            NotificationPreference: MapTo(notificationPreference: preference.NotificationPreference),
            GeneralPreference: MapTo(generalPreference: preference.GeneralPreference));
    }

    private static GetPreferencesResponseFinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferencesResponseFinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferencesResponseNotificationPreference? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferencesResponseNotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferencesResponseGeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferencesResponseGeneralPreference(UseDarkMode: generalPreference.UseDarkMode);
    }
}