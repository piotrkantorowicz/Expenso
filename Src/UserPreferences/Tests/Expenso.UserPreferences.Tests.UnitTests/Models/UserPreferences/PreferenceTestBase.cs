using Expenso.Shared.MessageBroker;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Models.UserPreferences;

internal abstract class PreferenceTestBase : TestBase
{
    protected Preference TestCandidate { get; private set; } = null!;

    protected Mock<IMessageBroker>? MessageBrokerMock { get; private set; }

    protected GeneralPreference DefaultGeneralPreference { get; } = GeneralPreference.Create(false);

    protected FinancePreference DefaultFinancePreference { get; } = FinancePreference.Create(false, 0, false, 0);

    protected NotificationPreference DefaultNotificationPreference { get; } = NotificationPreference.Create(true, 7);

    [SetUp]
    public void SetUp()
    {
        MessageBrokerMock = new Mock<IMessageBroker>();

        TestCandidate = Preference.Create(Guid.NewGuid(), Guid.NewGuid(), DefaultGeneralPreference,
            DefaultFinancePreference, DefaultNotificationPreference);
    }
}