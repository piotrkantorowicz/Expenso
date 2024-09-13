using Expenso.Communication.Proxy.DTO.Settings;
using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Communication.Proxy.DTO.Settings.InApp;
using Expenso.Communication.Proxy.DTO.Settings.Push;

namespace Expenso.Communication.Tests.UnitTests.Proxy.DTO.API.SendNotification.Extensions;

internal abstract class SendNotificationRequest_NotificationTypeExtensionsTestBase
{
    protected NotificationSettings _settings = null!;

    [SetUp]
    public void SetUp()
    {
        _settings = new NotificationSettings
        {
            Enabled = true,
            Email = new EmailNotificationSettings(Enabled: true,
                Smtp: new SmtpSettings(Host: String.Empty, Port: 8080, Ssl: false, Username: "ZinCai",
                    Password: "cC3iGXV"), From: "kennethsun@email.com", ReplyTo: "kennethsun@email.com"),
            InApp = new InAppNotificationSettings(Enabled: true),
            Push = new PushNotificationSettings(Enabled: true)
        };
    }
}