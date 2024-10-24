using FluentValidation;

using Keycloak.AuthServices.Common;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Keycloak;

internal sealed class CredentialsValidator : AbstractValidator<KeycloakClientInstallationCredentials>
{
    public CredentialsValidator()
    {
        RuleFor(expression: credentials => credentials.Secret)
            .NotEmpty()
            .WithMessage(errorMessage: "Client secret must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: credentials => credentials.Secret)
                .Must(predicate: secret => Guid.TryParse(input: secret, result: out _))
                .WithMessage(errorMessage: "Client secret must be a valid GUID format."));
    }
}