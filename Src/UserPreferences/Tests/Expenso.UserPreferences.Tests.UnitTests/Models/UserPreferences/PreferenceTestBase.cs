using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.UserPreferences;

internal abstract class PreferenceTestBase : TestBase<Preference>
{
    protected Mock<IMessageBroker> _messageBrokerMock = null!;
    protected readonly GeneralPreference _defaultGeneralPreference = GeneralPreference.Create(false);
    protected readonly FinancePreference _defaultFinancePreference = FinancePreference.Create(false, 0, false, 0);
    protected readonly NotificationPreference _defaultNotificationPreference = NotificationPreference.Create(true, 7);

    [SetUp]
    public void SetUp()
    {
        _messageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = Preference.Create(Guid.NewGuid(), Guid.NewGuid(), _defaultGeneralPreference,
            _defaultFinancePreference, _defaultNotificationPreference);
    }
}