using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

internal sealed class UpdatePreferenceRequestValidator : AbstractValidator<UpdatePreferenceRequest>
{
    public UpdatePreferenceRequestValidator(
        UpdatePreferenceRequest_FinancePreferenceValidator financePreferenceValidator,
        UpdatePreferenceRequest_NotificationPreferenceValidator notificationPreferenceValidator,
        UpdatePreferenceRequest_GeneralPreferenceValidator generalPreferenceValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: financePreferenceValidator,
            paramName: nameof(financePreferenceValidator));

        ArgumentNullException.ThrowIfNull(argument: notificationPreferenceValidator,
            paramName: nameof(notificationPreferenceValidator));

        ArgumentNullException.ThrowIfNull(argument: generalPreferenceValidator,
            paramName: nameof(generalPreferenceValidator));

        RuleFor(expression: x => x.FinancePreference)
            .NotNull()
            .WithMessage(errorMessage: "The finance preference must not be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.FinancePreference!).SetValidator(validator: financePreferenceValidator));

        RuleFor(expression: x => x.NotificationPreference)
            .NotNull()
            .WithMessage(errorMessage: "The notification preference must not be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.NotificationPreference!)
                    .SetValidator(validator: notificationPreferenceValidator));

        RuleFor(expression: x => x.GeneralPreference)
            .NotNull()
            .WithMessage(errorMessage: "The general preference must not be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.GeneralPreference!).SetValidator(validator: generalPreferenceValidator));
    }
}