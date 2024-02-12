using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

using TestCandidate = Expenso.UserPreferences.Core.Domain.Preferences.Model.Preference;

namespace Expenso.UserPreferences.Tests.UnitTests.Domain.Preferences.Model.Preference;

internal abstract class PreferenceTestBase : TestBase<TestCandidate>
{
    protected readonly FinancePreference _defaultFinancePreference = FinancePreference.Create(false, 0, false, 0);
    protected readonly GeneralPreference _defaultGeneralPreference = GeneralPreference.Create(false);
    protected readonly NotificationPreference _defaultNotificationPreference = NotificationPreference.Create(true, 7);
    protected Mock<IMessageBroker> _messageBrokerMock = null!;

    [SetUp]
    public void SetUp()
    {
        _messageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = TestCandidate.Create(PreferenceId.New(Guid.NewGuid()), UserId.New(Guid.NewGuid()),
            _defaultGeneralPreference, _defaultFinancePreference, _defaultNotificationPreference);
    }
}