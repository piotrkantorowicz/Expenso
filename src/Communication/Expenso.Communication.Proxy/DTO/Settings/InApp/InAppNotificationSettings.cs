using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Proxy.DTO.Settings.InApp;

public sealed record InAppNotificationSettings(bool? Enabled) : ISettings;