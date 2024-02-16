using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;

internal static class PreferenceFactory
{
    public static Preference Create(Guid userId, Guid? preferenceId = null)
    {
        return new Preference
        {
            Id = preferenceId ?? Guid.NewGuid(),
            UserId = userId,
            GeneralPreference = new GeneralPreference
            {
                UseDarkMode = false
            },
            NotificationPreference = new NotificationPreference
            {
                SendFinanceReportEnabled = true,
                SendFinanceReportInterval = 7
            },
            FinancePreference = new FinancePreference
            {
                AllowAddFinancePlanSubOwners = false,
                MaxNumberOfSubFinancePlanSubOwners = 0,
                AllowAddFinancePlanReviewers = false,
                MaxNumberOfFinancePlanReviewers = 0
            }
        };
    }
}