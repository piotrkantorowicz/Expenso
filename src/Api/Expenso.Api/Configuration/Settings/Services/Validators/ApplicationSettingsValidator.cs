using Expenso.Shared.Commands.Validation.Rules;
using Expenso.Shared.System.Configuration.Settings.App;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class ApplicationSettingsValidator : AbstractValidator<ApplicationSettings>
{
    public ApplicationSettingsValidator()
    {
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "Application settings are required.");

        RuleFor(expression: x => x.InstanceId)
            .NotNullOrEmpty()
            .WithMessage(errorMessage: "Instance ID must be provided and cannot be empty.");

        RuleFor(expression: x => x.Name)
            .NotEmpty()
            .WithMessage(errorMessage: "Name must be provided and cannot be empty.");

        RuleFor(expression: x => x.Version)
            .NotEmpty()
            .WithMessage(errorMessage: "Version must be provided and cannot be empty.")
            .DependentRules(action: () => RuleFor(expression: x => x.Version)
                .Must(predicate: (settings, version) =>
                {
                    string? assemblyVersion = typeof(Program).Assembly.GetName().Version?.ToString();

                    return Version.TryParse(input: version, result: out Version? settingsVer) &&
                           Version.TryParse(input: assemblyVersion, result: out Version? assemblyVer) &&
                           settingsVer.Major == assemblyVer.Major && settingsVer.Minor == assemblyVer.Minor &&
                           settingsVer.Build == assemblyVer.Build;
                })
                .WithMessage(messageProvider: settings =>
                {
                    string assemblyVersion = typeof(Program).Assembly.GetName().Version?.ToString()!;
                    string[] assemblyVersionParts = assemblyVersion.Split(separator: '.');

                    return
                        $"Version mismatch. Expected: [{assemblyVersionParts[0]}.{assemblyVersionParts[1]}.{assemblyVersionParts[2]}], but got: [{settings.Version}].";
                }));
    }
}