using Expenso.Api.Configuration.Settings.Services.Validators.Notifications;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.EmailSettingsValidator;

[TestFixture]
internal abstract class EmailNotificationSettingsValidatorTestBase : TestBase<EmailNotificationSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _emailNotificationSettings = new EmailNotificationSettings(Enabled: true,
            Smtp: new SmtpSettings(Host: "smtp.valid-host.com", Port: 587, Ssl: false, Username: "validuser",
                Password: "ValidPassword1!"), From: "valid@example.com", ReplyTo: "replyto@example.com");

        TestCandidate = new EmailNotificationSettingsValidator();
    }

    protected EmailNotificationSettings _emailNotificationSettings = null!;
}