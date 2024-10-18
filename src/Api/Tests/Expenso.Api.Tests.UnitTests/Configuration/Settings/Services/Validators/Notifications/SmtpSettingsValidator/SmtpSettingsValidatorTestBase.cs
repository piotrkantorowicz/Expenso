using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator;

[TestFixture]
internal abstract class
    SmtpSettingsValidatorTestBase : TestBase<
    Api.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _smtpSettings = new SmtpSettings(Host: "smtp.valid-host.com", Port: 587, Ssl: false, Username: "validuser",
            Password: "ValidPassword1!");

        TestCandidate = new Api.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator();
    }

    protected SmtpSettings _smtpSettings = null!;
}