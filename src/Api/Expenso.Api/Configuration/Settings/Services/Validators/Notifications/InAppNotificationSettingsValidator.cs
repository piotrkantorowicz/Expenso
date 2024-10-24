using Expenso.Communication.Shared.DTO.Settings.InApp;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class InAppNotificationSettingsValidator : AbstractValidator<InAppNotificationSettings>
{
    public InAppNotificationSettingsValidator()
    {
        RuleFor(expression: x => x.Enabled)
            .NotNull()
            .WithMessage(errorMessage: "In-app enabled flag must be provided.");
    }
}