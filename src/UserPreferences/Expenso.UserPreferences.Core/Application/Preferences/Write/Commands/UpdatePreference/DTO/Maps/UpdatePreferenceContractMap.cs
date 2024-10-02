using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.FinancePreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.GeneralPreferences;
using Expenso.UserPreferences.Proxy.DTO.MessageBus.UpdatePreference.NotificationPreferences;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceContractMap
{
    public static FinancePreferenceUpdatedIntegrationEventFinancePreference MapTo(FinancePreference financePreference)
    {
        return new FinancePreferenceUpdatedIntegrationEventFinancePreference(
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static NotificationPreferenceUpdatedIntegrationEventNotificationPreference MapTo(
        NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceUpdatedIntegrationEventNotificationPreference(
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    public static GeneralPreferenceUpdatedIntegrationEventGeneralPreference MapTo(GeneralPreference generalPreference)
    {
        return new GeneralPreferenceUpdatedIntegrationEventGeneralPreference(
            UseDarkMode: generalPreference.UseDarkMode);
    }
}