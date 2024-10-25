using Expenso.Shared.System.Metrics;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class OtlpSettingsValidator : AbstractValidator<OtlpSettings>
{
    public OtlpSettingsValidator()
    {
        RuleFor(expression: x => x.ServiceName)
            .NotEmpty()
            .WithMessage(errorMessage: "Service name must be provided and cannot be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.ServiceName)
                    .Must(predicate: x => x.IsAlphaNumericAndSpecialCharactersString(specialCharacters: "_.-"))
                    .WithMessage(
                        errorMessage: "Service name can only contain alphanumeric characters and special characters."));

        RuleFor(expression: x => x.Endpoint)
            .NotEmpty()
            .WithMessage(errorMessage: "Endpoint must be provided and cannot be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.Endpoint)
                    .Must(predicate: x => x.IsValidUrl())
                    .WithMessage(errorMessage: "Endpoint must be a valid URL."));
    }
}