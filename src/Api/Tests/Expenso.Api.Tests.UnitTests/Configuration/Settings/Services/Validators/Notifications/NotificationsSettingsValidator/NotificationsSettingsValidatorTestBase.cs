using Expenso.Api.Configuration.Settings.Services.Validators.Notifications;
using Expenso.Communication.Shared.DTO.Settings;
using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentValidation;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.
    NotificationsSettingsValidator;

[TestFixture]
internal abstract class NotificationSettingsValidatorTestBase : TestBase<NotificationSettingsValidator>
{
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

        Mock<IValidator<EmailNotificationSettings>> emailNotificationSettingsValidatorMock = new();
        Mock<IValidator<InAppNotificationSettings>> inAppNotificationSettingsValidatorMock = new();
        Mock<IValidator<PushNotificationSettings>> pushNotificationSettingsValidatorMock = new();

        TestCandidate = new NotificationSettingsValidator(
            emailNotificationSettingsValidator: emailNotificationSettingsValidatorMock.Object,
            inAppNotificationSettingsValidator: inAppNotificationSettingsValidatorMock.Object,
            pushNotificationSettingsValidator: pushNotificationSettingsValidatorMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _notificationSettings = null!;
        TestCandidate = null!;
    }

    protected NotificationSettings _notificationSettings = null!;
}