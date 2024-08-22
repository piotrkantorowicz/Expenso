using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Communication.Proxy.DTO.Settings.InApp;
using Expenso.Communication.Proxy.DTO.Settings.Push;

namespace Expenso.Communication.Proxy.DTO.Settings;

public sealed record NotificationSettings
{
    public bool? Enabled { get; init; }

    public EmailNotificationSettings Email { get; init; } = null!;

    public InAppNotificationSettings InApp { get; init; } = null!;

    public PushNotificationSettings Push { get; init; } = null!;
}