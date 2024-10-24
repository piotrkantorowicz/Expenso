using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.FinancePreferences.Payload;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.GeneralPreferences.Payload;
using Expenso.UserPreferences.Shared.DTO.MessageBus.UpdatePreference.NotificationPreferences.Payload;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Maps;

internal static class UpdatePreferenceContractMap
{
    public static FinancePreferenceUpdatedPayload MapTo(Guid userId, FinancePreference financePreference)
    {
        return new FinancePreferenceUpdatedPayload(UserId: userId,
            AllowAddFinancePlanSubOwners: financePreference.AllowAddFinancePlanSubOwners,
            MaxNumberOfSubFinancePlanSubOwners: financePreference.MaxNumberOfSubFinancePlanSubOwners,
            AllowAddFinancePlanReviewers: financePreference.AllowAddFinancePlanReviewers,
            MaxNumberOfFinancePlanReviewers: financePreference.MaxNumberOfFinancePlanReviewers);
    }

    public static NotificationPreferenceUpdatedPayload MapTo(Guid userId, NotificationPreference notificationPreference)
    {
        return new NotificationPreferenceUpdatedPayload(UserId: userId,
            SendFinanceReportEnabled: notificationPreference.SendFinanceReportEnabled,
            SendFinanceReportInterval: notificationPreference.SendFinanceReportInterval);
    }

    public static GeneralPreferenceUpdatedPayload MapTo(Guid userId, GeneralPreference generalPreference)
    {
        return new GeneralPreferenceUpdatedPayload(UserId: userId, UseDarkMode: generalPreference.UseDarkMode);
    }
}