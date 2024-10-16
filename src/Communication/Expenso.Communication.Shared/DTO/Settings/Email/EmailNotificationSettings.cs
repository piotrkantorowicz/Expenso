using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Shared.DTO.Settings.Email;

public sealed record EmailNotificationSettings(bool? Enabled, SmtpSettings? Smtp, string? From, string? ReplyTo)
    : ISettings;