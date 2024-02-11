using Expenso.Shared.Commands.Validation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandValidator : ICommandValidator<UpdatePreferenceCommand>
{
    public IDictionary<string, string> Validate(UpdatePreferenceCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(nameof(command), "Command cannot be null.");
        }

        if (command?.PreferenceOrUserId == Guid.Empty)
        {
            errors.Add(nameof(command.PreferenceOrUserId), "Preferences or user id cannot be empty.");
        }

        if (command?.Preference?.FinancePreference is null)
        {
            errors.Add(nameof(command.Preference.FinancePreference), "Finance preferences cannot be null.");
        }

        switch (command?.Preference?.FinancePreference?.MaxNumberOfFinancePlanReviewers)
        {
            case < 0:
                errors.Add(nameof(command.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    "Max number of finance plan reviewers cannot be negative.");

                break;
            case > 10:
                errors.Add(nameof(command.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    "Max number of finance plan reviewers cannot be greater than 10.");

                break;
        }

        switch (command?.Preference?.FinancePreference?.MaxNumberOfSubFinancePlanSubOwners)
        {
            case < 0:
                errors.Add(nameof(command.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    "Max number of sub finance plan sub owners cannot be negative.");

                break;
            case > 5:
                errors.Add(nameof(command.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    "Max number of sub finance plan sub owners cannot be greater than 5.");

                break;
        }

        if (command?.Preference?.NotificationPreference is null)
        {
            errors.Add(nameof(command.Preference.NotificationPreference), "Notification preferences cannot be null.");
        }

        switch (command?.Preference?.NotificationPreference?.SendFinanceReportInterval)
        {
            case < 0:
                errors.Add(nameof(command.Preference.NotificationPreference.SendFinanceReportInterval),
                    "Send finance report interval cannot be negative.");

                break;
            case > 31:
                errors.Add(nameof(command.Preference.NotificationPreference.SendFinanceReportInterval),
                    "Send finance report interval cannot be greater than 31.");

                break;
        }

        if (command?.Preference?.GeneralPreference is null)
        {
            errors.Add(nameof(command.Preference.GeneralPreference), "General preferences cannot be null.");
        }

        return errors;
    }
}