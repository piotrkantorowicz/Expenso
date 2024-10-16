using Expenso.Communication.Shared.DTO.Settings.Email;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate = Expenso.Api.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.SmtpSettingsValidator;

[TestFixture]
internal abstract class SmtpSettingsValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _smtpSettings = new SmtpSettings(Host: "smtp.valid-host.com", Port: 587, Ssl: false, Username: "validuser",
            Password: "ValidPassword1!");

        TestCandidate = new TestCandidate();
    }

    protected SmtpSettings _smtpSettings = null!;
}