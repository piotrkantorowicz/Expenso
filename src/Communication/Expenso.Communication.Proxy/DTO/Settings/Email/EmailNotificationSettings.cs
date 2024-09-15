namespace Expenso.Communication.Proxy.DTO.Settings.Email;

public sealed record EmailNotificationSettings(bool? Enabled, SmtpSettings? Smtp, string? From, string? ReplyTo);