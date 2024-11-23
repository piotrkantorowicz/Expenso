using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Maps;

internal static class GetPreferenceForCurrentUserResponseMap
{
    public static GetPreferenceForCurrentUserResponse MapTo(Preference preference)
    {
        return new GetPreferenceForCurrentUserResponse(Id: preference.Id, UserId: preference.UserId,
            FinancePreference: MapTo(financePreference: preference.FinancePreference),
            NotificationPreference: MapTo(notificationPreference: preference.NotificationPreference),
            GeneralPreference: MapTo(generalPreference: preference.GeneralPreference));
    }

    private static GetPreferenceForCurrentUserResponseFinancePreference? MapTo(FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new GetPreferenceForCurrentUserResponseFinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static GetPreferenceForCurrentUserResponseNotificationPreference? MapTo(
        NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new GetPreferenceForCurrentUserResponseNotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    private static GetPreferenceForCurrentUserResponseGeneralPreference? MapTo(GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GetPreferenceForCurrentUserResponseGeneralPreference(UseDarkMode: generalPreference.UseDarkMode);
    }
}