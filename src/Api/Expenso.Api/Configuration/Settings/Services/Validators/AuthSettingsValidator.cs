using Expenso.Shared.System.Configuration.Settings.Auth;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class AuthSettingsValidator : AbstractValidator<AuthSettings>
{
    public AuthSettingsValidator()
    {
        RuleFor(expression: x => x.AuthServer)
            .Must(predicate: authServer => Enum.IsDefined(enumType: typeof(AuthServer), value: authServer))
            .WithMessage(errorMessage: "AuthServer must be a valid value.");
    }
}