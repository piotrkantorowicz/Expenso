using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceRequestMap
{
    public static FinancePreference MapFrom(UpdateFinancePreferenceRequest updateFinancePreference)
    {
        return new FinancePreference
        {
            AllowAddFinancePlanReviewers = updateFinancePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers = updateFinancePreference.MaxNumberOfFinancePlanReviewers,
            AllowAddFinancePlanSubOwners = updateFinancePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners = updateFinancePreference.MaxNumberOfSubFinancePlanSubOwners
        };
    }

    public static GeneralPreference MapFrom(UpdateGeneralPreferenceRequest updateGeneralPreferenceRequest)
    {
        return new GeneralPreference
        {
            UseDarkMode = updateGeneralPreferenceRequest.UseDarkMode
        };
    }

    public static NotificationPreference MapFrom(
        UpdateNotificationPreferenceRequest updateNotificationPreferenceRequest)
    {
        return new NotificationPreference
        {
            SendFinanceReportEnabled = updateNotificationPreferenceRequest.SendFinanceReportEnabled,
            SendFinanceReportInterval = updateNotificationPreferenceRequest.SendFinanceReportInterval
        };
    }
}