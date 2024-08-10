using Expenso.Shared.Commands.Validation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference;

internal sealed class UpdatePreferenceCommandValidator : ICommandValidator<UpdatePreferenceCommand>
{
    public IDictionary<string, string> Validate(UpdatePreferenceCommand? command)
    {
        Dictionary<string, string> errors = new();

        if (command is null)
        {
            errors.Add(key: nameof(command), value: "Command is required");
        }

        if (command?.PreferenceOrUserId == Guid.Empty)
        {
            errors.Add(key: nameof(command.PreferenceOrUserId), value: "Preferences or user id cannot be empty");
        }

        if (command?.Preference?.FinancePreference is null)
        {
            errors.Add(key: nameof(command.Preference.FinancePreference), value: "Finance preferences cannot be null");
        }

        switch (command?.Preference?.FinancePreference?.MaxNumberOfFinancePlanReviewers)
        {
            case < 0:
                errors.Add(key: nameof(command.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    value: "Max number of finance plan reviewers cannot be negative");

                break;
            case > 10:
                errors.Add(key: nameof(command.Preference.FinancePreference.MaxNumberOfFinancePlanReviewers),
                    value: "Max number of finance plan reviewers cannot be greater than 10");

                break;
        }

        switch (command?.Preference?.FinancePreference?.MaxNumberOfSubFinancePlanSubOwners)
        {
            case < 0:
                errors.Add(key: nameof(command.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    value: "Max number of sub finance plan sub owners cannot be negative");

                break;
            case > 5:
                errors.Add(key: nameof(command.Preference.FinancePreference.MaxNumberOfSubFinancePlanSubOwners),
                    value: "Max number of sub finance plan sub owners cannot be greater than 5");

                break;
        }

        if (command?.Preference?.NotificationPreference is null)
        {
            errors.Add(key: nameof(command.Preference.NotificationPreference),
                value: "Notification preferences cannot be null");
        }

        switch (command?.Preference?.NotificationPreference?.SendFinanceReportInterval)
        {
            case < 0:
                errors.Add(key: nameof(command.Preference.NotificationPreference.SendFinanceReportInterval),
                    value: "Send finance report interval cannot be negative");

                break;
            case > 31:
                errors.Add(key: nameof(command.Preference.NotificationPreference.SendFinanceReportInterval),
                    value: "Send finance report interval cannot be greater than 31");

                break;
        }

        if (command?.Preference?.GeneralPreference is null)
        {
            errors.Add(key: nameof(command.Preference.GeneralPreference), value: "General preferences cannot be null");
        }

        return errors;
    }
}