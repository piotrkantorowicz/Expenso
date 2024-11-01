using Expenso.BudgetSharing.Application.Shared.Settings;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class BudgetSharingSettingsValidator : AbstractValidator<BudgetSharingSettings>
{
    private const int MinExpirationDays = 1;
    private const int MaxExpirationDays = 7;

    public BudgetSharingSettingsValidator()
    {
        RuleFor(expression: settings => settings.ExpirationDays)
            .NotEmpty()
            .WithMessage(errorMessage: "Expiration days must be provided.")
            .InclusiveBetween(from: MinExpirationDays, to: MaxExpirationDays)
            .WithMessage(errorMessage: "Expiration days must be between 1 and 7 days.");
    }
}