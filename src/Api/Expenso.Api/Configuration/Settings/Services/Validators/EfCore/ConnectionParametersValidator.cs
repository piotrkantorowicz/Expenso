using Expenso.Shared.Database.EfCore.Settings;
using Expenso.Shared.System.Types.TypesExtensions.Validations;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.EfCore;

internal sealed class ConnectionParametersValidator : AbstractValidator<ConnectionParameters>
{
    public ConnectionParametersValidator()
    {
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "Connection parameters settings are required.");

        RuleFor(expression: x => x.Host)
            .NotEmpty()
            .WithMessage(errorMessage: "Host must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Host)
                .Must(predicate: host => host.IsValidHost())
                .WithMessage(errorMessage: "Host must be a valid DNS name or IP address."));

        RuleFor(expression: x => x.Port)
            .NotEmpty()
            .WithMessage(errorMessage: "Port must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Port)
                .Must(predicate: port =>
                    int.TryParse(s: port, result: out int portNumber) && portNumber is >= 1 and <= 65535)
                .WithMessage(errorMessage: "Port must be a valid integer between 1 and 65535."));

        RuleFor(expression: x => x.DefaultDatabase)
            .NotEmpty()
            .WithMessage(errorMessage: "DefaultDatabase must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.DefaultDatabase)
                .Must(predicate: db =>
                    db.IsAlphaNumericAndSpecialCharactersString(minLength: 1, maxLength: 100, specialCharacters: "_-"))
                .WithMessage(
                    errorMessage: "DefaultDatabase must be an alphanumeric string between 1 and 100 characters."));

        RuleFor(expression: x => x.Database)
            .NotEmpty()
            .WithMessage(errorMessage: "Database must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Database)
                .Must(predicate: db =>
                    db.IsAlphaNumericAndSpecialCharactersString(minLength: 1, maxLength: 100, specialCharacters: "_-"))
                .WithMessage(errorMessage: "Database must be an alphanumeric string between 1 and 100 characters."));

        RuleFor(expression: x => x.User)
            .NotEmpty()
            .WithMessage(errorMessage: "User must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.User)
                .Must(predicate: user => user.IsValidUsername())
                .WithMessage(
                    errorMessage:
                    "User must be a valid alphanumeric string starting with a letter and between 3 and 30 characters."));

        RuleFor(expression: x => x.Password)
            .NotEmpty()
            .WithMessage(errorMessage: "Password must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Password)
                .Must(predicate: password => password.IsValidPassword(minLength: 8, maxLength: 30))
                .WithMessage(
                    errorMessage:
                    "Password must be between 8 and 30 characters, contain an upper and lower case letter, a digit, and a special character, with no spaces."));
    }
}