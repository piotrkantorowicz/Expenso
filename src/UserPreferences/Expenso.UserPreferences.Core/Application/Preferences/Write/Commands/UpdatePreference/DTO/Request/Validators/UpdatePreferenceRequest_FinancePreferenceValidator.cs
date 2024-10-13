using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

internal sealed class
    UpdatePreferenceRequest_FinancePreferenceValidator : AbstractValidator<UpdatePreferenceRequest_FinancePreference>
{
    public UpdatePreferenceRequest_FinancePreferenceValidator()
    {
        RuleFor(expression: x => x.MaxNumberOfFinancePlanReviewers)
            .InclusiveBetween(from: 0, to: 10)
            .WithMessage(errorMessage: "The number of finance plan reviewers must be between 0 and 10.");

        RuleFor(expression: x => x.MaxNumberOfSubFinancePlanSubOwners)
            .InclusiveBetween(from: 0, to: 5)
            .WithMessage(errorMessage: "The number of finance plan sub-owners must be between 0 and 5.");
    }
}