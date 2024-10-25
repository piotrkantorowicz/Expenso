using Expenso.Communication.Shared.DTO.Settings.Push;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class PushNotificationSettingsValidator : AbstractValidator<PushNotificationSettings>
{
    public PushNotificationSettingsValidator()
    {
        RuleFor(expression: x => x.Enabled).NotNull().WithMessage(errorMessage: "Push enabled flag must be provided.");
    }
}