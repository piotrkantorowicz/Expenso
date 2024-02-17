using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;

internal static class PreferenceFactory
{
    public static Preference Create(Guid userId, Guid? preferenceId = null)
    {
        Guid id = preferenceId ?? Guid.NewGuid();

        return new Preference
        {
            Id = id,
            UserId = userId,
            GeneralPreference = new GeneralPreference
            {
                Id = Guid.NewGuid(),
                PreferenceId = id,
                UseDarkMode = false
            },
            NotificationPreference = new NotificationPreference
            {
                Id = Guid.NewGuid(),
                PreferenceId = id,
                SendFinanceReportEnabled = true,
                SendFinanceReportInterval = 7
            },
            FinancePreference = new FinancePreference
            {
                Id = Guid.NewGuid(),
                PreferenceId = id,
                AllowAddFinancePlanSubOwners = false,
                MaxNumberOfSubFinancePlanSubOwners = 0,
                AllowAddFinancePlanReviewers = false,
                MaxNumberOfFinancePlanReviewers = 0
            }
        };
    }
}