using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Api.Configuration.Settings.Services.Validators.Notifications.InAppNotificationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.InAppSettingsValidator;

[TestFixture]
internal abstract class InAppNotificationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    [SetUp]
    public void SetUp()
    {
        _inAppNotificationSettings = new InAppNotificationSettings(Enabled: true);
        TestCandidate = new TestCandidate();
    }

    protected InAppNotificationSettings _inAppNotificationSettings = null!;
}