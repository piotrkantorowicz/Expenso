using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceRequestMap
{
    public static FinancePreference? MapFrom(UpdatePreferenceRequest_FinancePreference? financePreference)
    {
        if (financePreference is null)
        {
            return null;
        }

        return new FinancePreference
        {
            AllowAddFinancePlanReviewers = financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers = financePreference.MaxNumberOfFinancePlanReviewers,
            AllowAddFinancePlanSubOwners = financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners = financePreference.MaxNumberOfSubFinancePlanSubOwners
        };
    }

    public static GeneralPreference? MapFrom(UpdatePreferenceRequest_GeneralPreference? generalPreference)
    {
        if (generalPreference is null)
        {
            return null;
        }

        return new GeneralPreference
        {
            UseDarkMode = generalPreference.UseDarkMode
        };
    }

    public static NotificationPreference? MapFrom(
        UpdatePreferenceRequest_NotificationPreference? notificationPreference)
    {
        if (notificationPreference is null)
        {
            return null;
        }

        return new NotificationPreference
        {
            SendFinanceReportEnabled = notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval = notificationPreference.SendFinanceReportInterval
        };
    }
}