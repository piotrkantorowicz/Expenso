using Expenso.Shared.Database.EfCore.Settings;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.EfCore;

internal sealed class EfCoreSettingsValidator : AbstractValidator<EfCoreSettings>
{
    public EfCoreSettingsValidator(ConnectionParametersValidator connectionParametersValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: connectionParametersValidator);
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "EfCore settings are required.");

        RuleFor(expression: x => x.ConnectionParameters)
            .NotNull()
            .WithMessage(errorMessage: "ConnectionParameters must be provided and cannot be null.")
            .DependentRules(action: () =>
            {
                RuleFor(expression: x => x.ConnectionParameters!)
                    .SetValidator(validator: connectionParametersValidator);
            });

        RuleFor(expression: x => x.InMemory).NotNull().WithMessage(errorMessage: "InMemory flag must be provided.");

        RuleFor(expression: x => x.UseMigration)
            .NotNull()
            .WithMessage(errorMessage: "UseMigration flag must be provided.");

        RuleFor(expression: x => x.UseSeeding).NotNull().WithMessage(errorMessage: "UseSeeding flag must be provided.");
    }
}