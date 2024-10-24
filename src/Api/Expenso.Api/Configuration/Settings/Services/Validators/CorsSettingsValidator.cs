using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class CorsSettingsValidator : AbstractValidator<CorsSettings>
{
    public CorsSettingsValidator()
    {
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "Cors settings are required.");
        RuleFor(expression: x => x.Enabled).NotNull().WithMessage(errorMessage: "Cors enabled flag must be provided.");

        When(predicate: x => x.Enabled == true, action: () =>
        {
            RuleFor(expression: x => x.AllowedOrigins)
                .NotEmpty()
                .WithMessage(errorMessage: "AllowedOrigins cannot be null or empty.");

            RuleForEach(expression: x => x.AllowedOrigins)
                .Must(predicate: origin =>
                    !string.IsNullOrWhiteSpace(value: origin) && (origin == "*" || origin.IsValidUrl()))
                .WithMessage(errorMessage: "Origin cannot be empty and must be a valid URL.");
        });
    }
}