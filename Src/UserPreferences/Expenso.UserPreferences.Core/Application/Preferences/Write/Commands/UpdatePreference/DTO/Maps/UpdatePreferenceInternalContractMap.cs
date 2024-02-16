using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceInternalContractMap
{
    public static FinancePreferenceContract MapTo(FinancePreference financePreference)
    {
        return new FinancePreferenceContract(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static NotificationPreferenceContract MapTo(NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceContract(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    public static GeneralPreferenceContract MapTo(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceContract(generalPreference.UseDarkMode);
    }
}