using Expenso.UserPreferences.Core.Application.Proxy;
using Expenso.UserPreferences.Core.Application.Services;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Proxy;

internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    protected Mock<IPreferencesService> _preferenceServiceMock = null!;
    protected PreferenceContract _preferenceContract = null!;
    protected Guid _userId = Guid.NewGuid();

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();

        _preferenceContract = new PreferenceContract(Guid.NewGuid(), _userId,
            new FinancePreferenceContract(false, 0, false, 0), new NotificationPreferenceContract(true, 7),
            new GeneralPreferenceContract(false));

        _preferenceServiceMock = new Mock<IPreferencesService>();
        TestCandidate = new UserPreferencesProxy(_preferenceServiceMock.Object);
    }
}