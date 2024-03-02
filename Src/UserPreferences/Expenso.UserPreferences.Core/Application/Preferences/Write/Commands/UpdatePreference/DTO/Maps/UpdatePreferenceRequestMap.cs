using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceRequestMap
{
    public static FinancePreference MapFrom(UpdatePreferenceRequest_FinancePreference financePreference)
    {
        return new FinancePreference
        {
            AllowAddFinancePlanReviewers = financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers = financePreference.MaxNumberOfFinancePlanReviewers,
            AllowAddFinancePlanSubOwners = financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners = financePreference.MaxNumberOfSubFinancePlanSubOwners
        };
    }

    public static GeneralPreference MapFrom(UpdatePreferenceRequest_GeneralPreference generalGeneralPreference)
    {
        return new GeneralPreference
        {
            UseDarkMode = generalGeneralPreference.UseDarkMode
        };
    }

    public static NotificationPreference MapFrom(UpdatePreferenceRequest_NotificationPreference updatePreferenceRequest)
    {
        return new NotificationPreference
        {
            SendFinanceReportEnabled = updatePreferenceRequest.SendFinanceReportEnabled,
            SendFinanceReportInterval = updatePreferenceRequest.SendFinanceReportInterval
        };
    }
}