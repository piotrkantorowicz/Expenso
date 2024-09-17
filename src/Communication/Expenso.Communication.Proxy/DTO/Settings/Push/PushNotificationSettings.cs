using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Proxy.DTO.Settings.Push;

public sealed record PushNotificationSettings(bool? Enabled) : ISettings;