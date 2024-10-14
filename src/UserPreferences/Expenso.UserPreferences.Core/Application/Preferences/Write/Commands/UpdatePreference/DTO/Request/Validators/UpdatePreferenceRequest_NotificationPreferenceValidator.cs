using FluentValidation;

namespace Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.UpdatePreference.DTO.Request.Validators;

internal sealed class
    UpdatePreferenceRequest_NotificationPreferenceValidator : AbstractValidator<
    UpdatePreferenceRequest_NotificationPreference>
{
    public UpdatePreferenceRequest_NotificationPreferenceValidator()
    {
        RuleFor(expression: x => x.SendFinanceReportInterval)
            .InclusiveBetween(from: 0, to: 31)
            .WithMessage(errorMessage: "The interval for sending the finance report must be between 0 and 31 days.");
    }
}