using Expenso.Api.Configuration.Settings.Services.Validators.Notifications;
using Expenso.Communication.Shared.DTO.Settings.Push;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.PushSettingsValidator;

[TestFixture]
internal abstract class PushNotificationSettingsValidatorTestBase : TestBase<PushNotificationSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _pushNotificationSettings = new PushNotificationSettings(Enabled: true);
        TestCandidate = new PushNotificationSettingsValidator();
    }

    protected PushNotificationSettings _pushNotificationSettings = null!;
}