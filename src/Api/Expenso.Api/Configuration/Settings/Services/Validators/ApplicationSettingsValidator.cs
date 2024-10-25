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
                .Must(predicate: (_, version) =>
                {
                    Version? assemblyVersion = typeof(Program).Assembly.GetName().Version;

                    if (assemblyVersion == null || !Version.TryParse(input: version, result: out Version? settingsVer))
                    {
                        return false;
                    }

                    return settingsVer.Major == assemblyVersion.Major && settingsVer.Minor == assemblyVersion.Minor &&
                           settingsVer.Build == assemblyVersion.Build;
                })
                .WithMessage(messageProvider: settings =>
                {
                    Version assemblyVersion = typeof(Program).Assembly.GetName().Version!;

                    return
                        $"Version mismatch. Expected: [{assemblyVersion.Major}.{assemblyVersion.Minor}.{assemblyVersion.Build}], but got: [{settings.Version}].";
                }));
    }
}