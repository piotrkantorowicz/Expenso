using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;

internal static class PreferenceFactory
{
    public static Preference Create(Guid? preferenceId, Guid userId)
    {
        Guid id = preferenceId ?? Guid.CreateVersion7();

        return new Preference
        {
            Id = id,
            UserId = userId,
            GeneralPreference = new GeneralPreference
            {
                Id = Guid.CreateVersion7(),
                PreferenceId = id,
                UseDarkMode = false
            },
            NotificationPreference = new NotificationPreference
            {
                Id = Guid.CreateVersion7(),
                PreferenceId = id,
                SendFinanceReportEnabled = true,
                SendFinanceReportInterval = 7
            },
            FinancePreference = new FinancePreference
            {
                Id = Guid.CreateVersion7(),
                PreferenceId = id,
                AllowAddFinancePlanSubOwners = false,
                MaxNumberOfSubFinancePlanSubOwners = 0,
                AllowAddFinancePlanReviewers = false,
                MaxNumberOfFinancePlanReviewers = 0
            }
        };
    }
}