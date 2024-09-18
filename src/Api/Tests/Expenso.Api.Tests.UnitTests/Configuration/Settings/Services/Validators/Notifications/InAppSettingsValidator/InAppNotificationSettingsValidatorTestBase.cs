using Expenso.Communication.Proxy.DTO.Settings.InApp;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Api.Configuration.Settings.Services.Validators.Notifications.InAppNotificationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.InAppSettingsValidator;

internal abstract class InAppNotificationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected InAppNotificationSettings _inAppNotificationSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _inAppNotificationSettings = new InAppNotificationSettings(Enabled: true);
        TestCandidate = new TestCandidate();
    }
}