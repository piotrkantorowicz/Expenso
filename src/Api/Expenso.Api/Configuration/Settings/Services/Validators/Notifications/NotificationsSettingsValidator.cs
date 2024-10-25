using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class NotificationSettingsValidator : AbstractValidator<NotificationSettings>
{
    public NotificationSettingsValidator(IValidator<EmailNotificationSettings> emailNotificationSettingsValidator,
        IValidator<InAppNotificationSettings> inAppNotificationSettingsValidator,
        IValidator<PushNotificationSettings> pushNotificationSettingsValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: emailNotificationSettingsValidator);
        ArgumentNullException.ThrowIfNull(argument: inAppNotificationSettingsValidator);
        ArgumentNullException.ThrowIfNull(argument: pushNotificationSettingsValidator);
        RuleFor(expression: x => x).NotNull().WithMessage(errorMessage: "Notification settings are required.");
        RuleFor(expression: x => x.Enabled).NotNull().WithMessage(errorMessage: "Enabled flag must be provided.");

        When(predicate: x => x.Enabled == true, action: () =>
        {
            RuleFor(expression: x => x.Email)
                .NotNull()
                .WithMessage(errorMessage: "Email notification settings must be provided.")
                .DependentRules(action: () =>
                    RuleFor(expression: x => x.Email!).SetValidator(validator: emailNotificationSettingsValidator));

            RuleFor(expression: x => x.InApp)
                .NotNull()
                .WithMessage(errorMessage: "In-app notification settings must be provided.")
                .DependentRules(action: () =>
                    RuleFor(expression: x => x.InApp!).SetValidator(validator: inAppNotificationSettingsValidator));

            RuleFor(expression: x => x.Push)
                .NotNull()
                .WithMessage(errorMessage: "Push notification settings must be provided.")
                .DependentRules(action: () =>
                    RuleFor(expression: x => x.Push!).SetValidator(validator: pushNotificationSettingsValidator));
        });
    }
}