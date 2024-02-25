using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceContractMap
{
    public static FinancePreferenceUpdatedIntegrationEvent_FinancePreference MapTo(FinancePreference financePreference)
    {
        return new FinancePreferenceUpdatedIntegrationEvent_FinancePreference(
            financePreference.AllowAddFinancePlanSubOwners, financePreference.MaxNumberOfSubFinancePlanSubOwners,
            financePreference.AllowAddFinancePlanReviewers, financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static NotificationPreferenceUpdatedIntegrationEvent_NotificationPreference MapTo(
        NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceUpdatedIntegrationEvent_NotificationPreference(
            notificationPreference.SendFinanceReportEnabled, notificationPreference.SendFinanceReportInterval);
    }

    public static GeneralPreferenceUpdatedIntegrationEvent_GeneralPreference MapTo(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceUpdatedIntegrationEvent_GeneralPreference(generalPreference.UseDarkMode);
    }
}