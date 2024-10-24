using Expenso.IAM.Core.Acl.Keycloak;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Keycloak;

internal sealed class KeycloakSettingsValidator : AbstractValidator<KeycloakSettings>
{
    private readonly string[] _sslRequiredValues =
    [
        "ALL",
        "EXTERNAL",
        "NONE"
    ];

    public KeycloakSettingsValidator(CredentialsValidator credentialsValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: credentialsValidator);
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "Keycloak settings are required.");

        RuleFor(expression: x => x.AuthServerUrl)
            .NotEmpty()
            .WithMessage(errorMessage: "Authorization server URL must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.AuthServerUrl)
                .Must(predicate: url => url.IsValidUrl())
                .WithMessage(errorMessage: "Authorization server URL must be a valid HTTP or HTTPS URL."));

        RuleFor(expression: x => x.Realm)
            .NotEmpty()
            .WithMessage(errorMessage: "Realm must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Realm)
                .Must(predicate: realm => realm.IsAlphaNumericString(minLength: 5, maxLength: 50))
                .WithMessage(errorMessage: "Realm must be an alpha string with a length between 5 and 50 characters."));

        RuleFor(expression: x => x.Resource)
            .NotEmpty()
            .WithMessage(errorMessage: "Resource (client ID) must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Resource)
                .Must(predicate: resource =>
                    resource.IsAlphaNumericAndSpecialCharactersString(minLength: 5, maxLength: 100,
                        specialCharacters: "_.-"))
                .WithMessage(
                    errorMessage:
                    "Resource (client ID) must be an alpha string with a length between 5 and 100 characters."));

        RuleFor(expression: x => x.SslRequired)
            .NotEmpty()
            .WithMessage(errorMessage: "SSL requirement must be specified and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.SslRequired)
                .Must(predicate: sslRequired =>
                    _sslRequiredValues.Contains(value: sslRequired,
                        comparer: StringComparer.InvariantCultureIgnoreCase))
                .WithMessage(errorMessage: "SSL requirement must be one of the predefined values."));

        RuleFor(expression: x => x.VerifyTokenAudience)
            .NotNull()
            .WithMessage(errorMessage: "Token audience must be specified.");

        RuleFor(expression: x => x.Credentials)
            .NotNull()
            .WithMessage(errorMessage: "Client secret must be provided and cannot be empty.")
            .DependentRules(action: () =>
                RuleFor(expression: x => x.Credentials).SetValidator(validator: credentialsValidator));
    }
}
