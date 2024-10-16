using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.System.Configuration.Settings;

namespace Expenso.Communication.Shared.DTO.Settings;

public sealed record NotificationSettings : ISettings
{
    public bool? Enabled { get; init; }

    public EmailNotificationSettings? Email { get; init; }

    public InAppNotificationSettings? InApp { get; init; }

    public PushNotificationSettings? Push { get; init; }
}