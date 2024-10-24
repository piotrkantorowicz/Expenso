using Expenso.Communication.Shared.DTO.Settings.Email;

using FluentValidation;

namespace Expenso.Api.Configuration.Settings.Services.Validators.Notifications;

internal sealed class EmailNotificationSettingsValidator : AbstractValidator<EmailNotificationSettings>
{
    public EmailNotificationSettingsValidator(SmtpSettingsValidator smtpSettingsValidator)
    {
        ArgumentNullException.ThrowIfNull(argument: smtpSettingsValidator);
        RuleFor(expression: x => x.Enabled).NotNull().WithMessage(errorMessage: "Email enabled flag must be provided.");

        When(predicate: x => x.Enabled == true, action: () =>
        {
            RuleFor(expression: x => x.From)
                .NotEmpty()
                .WithMessage(errorMessage: "Email 'From' address must be provided and cannot be empty.")
                .DependentRules(action: () => RuleFor(expression: x => x.From)
                    .EmailAddress()
                    .WithMessage(errorMessage: "Email 'From' address must be a valid email address."));

            RuleFor(expression: x => x.ReplyTo)
                .NotEmpty()
                .WithMessage(errorMessage: "Email 'ReplyTo' address must be provided and cannot be empty.")
                .DependentRules(action: () => RuleFor(expression: x => x.ReplyTo)
                    .EmailAddress()
                    .WithMessage(errorMessage: "Email 'ReplyTo' address must be a valid email address."));

            RuleFor(expression: x => x.Smtp)
                .NotNull()
                .WithMessage(errorMessage: "Smtp must be provided and cannot be null.")
                .DependentRules(action: () => RuleFor(expression: x => x.Smtp!)
                    .SetValidator(validator: smtpSettingsValidator));
        });
    }
}