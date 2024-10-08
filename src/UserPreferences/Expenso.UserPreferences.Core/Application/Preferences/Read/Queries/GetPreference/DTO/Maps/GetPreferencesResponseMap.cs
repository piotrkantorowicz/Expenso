using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

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

    private static GetPreferenceResponse_FinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_FinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferenceResponse_NotificationPreference? MapTo(NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_NotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferenceResponse_GeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferenceResponse_GeneralPreference(UseDarkMode: generalPreference.UseDarkMode);
    }
}