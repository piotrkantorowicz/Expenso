using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class SmtpSettingsValidator : AbstractValidator<SmtpSettings>
{
    public SmtpSettingsValidator()
    {
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "SMTP settings are required.");

        RuleFor(expression: x => x.Host)
            .NotEmpty()
            .WithMessage(errorMessage: "SMTP host must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Host)
                .Must(predicate: host => host.IsValidHost())
                .WithMessage(errorMessage: "SMTP host must be a valid DNS name, IPv4, or IPv6 address."));

        RuleFor(expression: x => x.Port)
            .GreaterThan(valueToCompare: 0)
            .WithMessage(errorMessage: "SMTP port must be a valid integer between 1 and 65535.");

        RuleFor(expression: x => x.Username)
            .NotEmpty()
            .WithMessage(errorMessage: "SMTP username must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Username)
                .Must(predicate: username => username.IsValidUsername())
                .WithMessage(
                    errorMessage: "SMTP username must be between 3 and 30 characters long and start with a letter."));

        RuleFor(expression: x => x.Password)
            .NotEmpty()
            .WithMessage(errorMessage: "SMTP password must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Password)
                .Must(predicate: password => password.IsValidPassword(minLength: 8, maxLength: 20))
                .WithMessage(
                    errorMessage:
                    "SMTP password must be between 8 and 20 characters long, with at least one uppercase letter, one lowercase letter, one digit, and one special character."));
    }
}