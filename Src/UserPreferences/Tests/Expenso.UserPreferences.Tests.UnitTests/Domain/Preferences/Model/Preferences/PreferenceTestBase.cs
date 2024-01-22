using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.Preferences;

internal abstract class PreferenceTestBase : TestBase<Preference>
{
    protected readonly FinancePreference _defaultFinancePreference = FinancePreference.Create(false, 0, false, 0);
    protected readonly GeneralPreference _defaultGeneralPreference = GeneralPreference.Create(false);
    protected readonly NotificationPreference _defaultNotificationPreference = NotificationPreference.Create(true, 7);
    protected Mock<IMessageBroker> _messageBrokerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _messageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = Preference.Create(Guid.NewGuid(), Guid.NewGuid(), _defaultGeneralPreference,
            _defaultFinancePreference, _defaultNotificationPreference);
    }
}