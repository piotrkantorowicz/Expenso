using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Shared.DTO.Settings.Email;

public sealed record SmtpSettings(string? Host, int? Port, bool? Ssl, string? Username, string? Password) : ISettings;