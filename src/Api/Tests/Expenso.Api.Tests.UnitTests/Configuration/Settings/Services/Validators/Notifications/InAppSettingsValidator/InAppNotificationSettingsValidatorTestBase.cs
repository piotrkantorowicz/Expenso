using Expenso.Api.Configuration.Settings.Services.Validators.Notifications;
using Expenso.Communication.Shared.DTO.Settings.InApp;
using Expenso.Shared.Tests.Utils.UnitTests;

namespace Expenso.Api.Tests.UnitTests.Configuration.Settings.Services.Validators.Notifications.InAppSettingsValidator;

[TestFixture]
internal abstract class InAppNotificationSettingsValidatorTestBase : TestBase<InAppNotificationSettingsValidator>
{
    [SetUp]
    public void SetUp()
    {
        _inAppNotificationSettings = new InAppNotificationSettings(Enabled: true);
        TestCandidate = new InAppNotificationSettingsValidator();
    }

    protected InAppNotificationSettings _inAppNotificationSettings = null!;
}