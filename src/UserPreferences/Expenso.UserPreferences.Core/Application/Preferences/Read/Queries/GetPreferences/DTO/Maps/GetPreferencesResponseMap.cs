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

    private static GetPreferencesResponse_FinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferencesResponse_FinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferencesResponse_NotificationPreference? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferencesResponse_NotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferencesResponse_GeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferencesResponse_GeneralPreference(UseDarkMode: generalPreference.UseDarkMode);
    }
}