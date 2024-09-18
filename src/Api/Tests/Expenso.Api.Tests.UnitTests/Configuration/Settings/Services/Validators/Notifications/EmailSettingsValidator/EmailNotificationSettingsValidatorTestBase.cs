using Expenso.Communication.Proxy.DTO.Settings.Email;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Api.Configuration.Settings.Services.Validators.Notifications.EmailNotificationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.EmailSettingsValidator;

internal abstract class EmailNotificationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected EmailNotificationSettings _emailNotificationSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _emailNotificationSettings = new EmailNotificationSettings(Enabled: true,
            Smtp: new SmtpSettings(Host: "smtp.valid-host.com", Port: 587, Ssl: false, Username: "validuser",
                Password: "ValidPassword1!"), From: "valid@example.com", ReplyTo: "replyto@example.com");

        TestCandidate = new TestCandidate();
    }
}