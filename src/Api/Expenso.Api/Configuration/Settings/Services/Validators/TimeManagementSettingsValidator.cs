using Expenso.TimeManagement.Core.Application.Shared.Settings;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators;

internal sealed class TimeManagementSettingsValidator : AbstractValidator<TimeManagementSettings>
{
    public TimeManagementSettingsValidator()
    {
        RuleFor(expression: x => x.AllowedEvents)
            .NotNull()
            .WithMessage(errorMessage: "Allowed events must be provided.")
            .Must(predicate: x => x?.Length <= 100)
            .WithMessage(errorMessage: "Too many allowed events specified. Maximum is 100.")
            .ForEach(action: x => x.IsInEnum().WithMessage(errorMessage: "Allowed events must be valid values."));
    }
}