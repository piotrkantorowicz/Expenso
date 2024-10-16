using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Api.Configuration.Settings.Services.Validators.Notifications.NotificationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.
    NotificationsSettingsValidator;

internal abstract class NotificationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected NotificationSettings _notificationSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _notificationSettings = new NotificationSettings
        {
            Enabled = true,
            Email = new EmailNotificationSettings(Enabled: true,
                Smtp: new SmtpSettings(Host: "smtp.valid-host.com", Port: 587, Ssl: false, Username: "validuser",
                    Password: "ValidPassword1!"), From: "valid@example.com", ReplyTo: "replyto@example.com"),
            InApp = new InAppNotificationSettings(Enabled: true),
            Push = new PushNotificationSettings(Enabled: true)
        };

        TestCandidate = new TestCandidate();
    }
}