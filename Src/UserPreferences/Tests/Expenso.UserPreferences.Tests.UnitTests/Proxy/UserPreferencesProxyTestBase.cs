using Expenso.UserPreferences.Core.Proxy;
using Expenso.UserPreferences.Core.Services;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Proxy;

internal abstract class UserPreferencesProxyTestBase
{
    protected IUserPreferencesProxy TestCandidate { get; private set; } = null!;

    protected Mock<IPreferencesService> PreferenceServiceMock { get; private set; } = null!;

    protected PreferenceContract PreferenceContract { get; private set; } = null!;

    protected Guid UserId { get; private set; }

    [SetUp]
    public void SetUp()
    {
        UserId = Guid.NewGuid();

        PreferenceContract = new PreferenceContract(Guid.NewGuid(), UserId,
            new FinancePreferenceContract(false, 0, false, 0), new NotificationPreferenceContract(true, 7),
            new GeneralPreferenceContract(false));

        PreferenceServiceMock = new Mock<IPreferencesService>();
        TestCandidate = new UserPreferencesProxy(PreferenceServiceMock.Object);
    }
}