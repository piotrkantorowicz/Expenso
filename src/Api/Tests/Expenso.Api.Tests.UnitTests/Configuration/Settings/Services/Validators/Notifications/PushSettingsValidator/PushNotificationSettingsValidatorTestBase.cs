using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

using TestCandidate =
    Expenso.Api.Configuration.Settings.Services.Validators.Notifications.PushNotificationSettingsValidator;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.PushSettingsValidator;

internal abstract class PushNotificationSettingsValidatorTestBase : TestBase<TestCandidate>
{
    protected PushNotificationSettings _pushNotificationSettings = null!;

    [SetUp]
    public void SetUp()
    {
        _pushNotificationSettings = new PushNotificationSettings(Enabled: true);
        TestCandidate = new TestCandidate();
    }
}