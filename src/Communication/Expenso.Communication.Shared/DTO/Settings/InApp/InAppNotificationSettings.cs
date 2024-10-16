using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Shared.DTO.Settings.InApp;

public sealed record InAppNotificationSettings(bool? Enabled) : ISettings;