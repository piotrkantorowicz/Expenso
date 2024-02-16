using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External.DTO.Maps;

internal static class CreatePreferenceResponseMap
{
    public static CreatePreferenceResponse MapTo(Preference preference)
    {
        return new CreatePreferenceResponse(preference.Id, preference.UserId, MapTo(preference.FinancePreference!),
            MapTo(preference.NotificationPreference!), MapTo(preference.GeneralPreference!));
    }

    private static CreateFinancePreferenceResponse MapTo(FinancePreference financePreference)
    {
        return new CreateFinancePreferenceResponse(financePreference.AllowAddFinancePlanSubOwners,
            financePreference.MaxNumberOfSubFinancePlanSubOwners, financePreference.AllowAddFinancePlanReviewers,
            financePreference.MaxNumberOfFinancePlanReviewers);
    }

    private static CreateNotificationPreferenceResponse MapTo(NotificationPreference notificationPreference)
    {
        return new CreateNotificationPreferenceResponse(notificationPreference.SendFinanceReportEnabled,
            notificationPreference.SendFinanceReportInterval);
    }

    private static CreateGeneralPreferenceResponse MapTo(GeneralPreference generalPreference)
    {
        return new CreateGeneralPreferenceResponse(generalPreference.UseDarkMode);
    }
}