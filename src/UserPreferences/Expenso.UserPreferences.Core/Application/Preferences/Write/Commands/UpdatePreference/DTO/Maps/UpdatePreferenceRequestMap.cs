using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Requests;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceRequestMap
{
    public static FinancePreference MapFrom(UpdatePreferenceRequestFinancePreference financePreference)
    {
        return new FinancePreference
        {
            AllowAddFinancePlanReviewers = financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers = financePreference.MaxNumberOfFinancePlanReviewers,
            AllowAddFinancePlanSubOwners = financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners = financePreference.MaxNumberOfSubFinancePlanSubOwners
        };
    }

    public static GeneralPreference MapFrom(UpdatePreferenceRequestGeneralPreference generalGeneralPreference)
    {
        return new GeneralPreference
        {
            UseDarkMode = generalGeneralPreference.UseDarkMode
        };
    }

    public static NotificationPreference MapFrom(UpdatePreferenceRequestNotificationPreference updatePreferenceRequest)
    {
        return new NotificationPreference
        {
            SendFinanceReportEnabled = updatePreferenceRequest.SendFinanceReportEnabled,
            SendFinanceReportInterval = updatePreferenceRequest.SendFinanceReportInterval
        };
    }
}