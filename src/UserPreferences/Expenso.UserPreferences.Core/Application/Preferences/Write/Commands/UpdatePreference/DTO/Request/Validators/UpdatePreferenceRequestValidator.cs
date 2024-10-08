using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

public sealed class UpdatePreferenceRequestValidator : AbstractValidator<UpdatePreferenceRequest>
{
    public UpdatePreferenceRequestValidator()
    {
        RuleFor(expression: x => x.FinancePreference)
            .NotNull()
            .WithMessage(errorMessage: "Finance preference cannot be empty.");

        RuleFor(expression: x => x.NotificationPreference)
            .NotNull()
            .WithMessage(errorMessage: "Notification preference cannot be empty.");

        RuleFor(expression: x => x.GeneralPreference)
            .NotNull()
            .WithMessage(errorMessage: "General preference cannot be empty.");

        When(predicate: x => x.FinancePreference != null, action: () =>
        {
            RuleFor(expression: x => x.FinancePreference!.MaxNumberOfFinancePlanReviewers)
                .InclusiveBetween(from: 0, to: 10)
                .WithMessage(errorMessage: "Max number of finance plan reviewers must be between 0 and 10.");

            RuleFor(expression: x => x.FinancePreference!.MaxNumberOfSubFinancePlanSubOwners)
                .InclusiveBetween(from: 0, to: 5)
                .WithMessage(errorMessage: "Max number of sub finance plan sub owners must be between 0 and 5.");
        });

        When(predicate: x => x.NotificationPreference != null, action: () =>
        {
            RuleFor(expression: x => x.NotificationPreference!.SendFinanceReportInterval)
                .InclusiveBetween(from: 0, to: 31)
                .WithMessage(errorMessage: "Send finance report interval must be between 0 and 31.");
        });
    }
}