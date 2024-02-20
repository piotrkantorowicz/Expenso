using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

using TestCandidate = Expenso.UserPreferences.Core.Application.Preferences.Proxy.UserPreferencesProxy;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected GetPreferenceResponse _getPreferenceResponse = null!;
    private Guid _id;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _id = Guid.NewGuid();
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        _commandDispatcherMock = new Mock<ICommandDispatcher>();

        _getPreferenceResponse = new GetPreferenceResponse(new GetFinancePreferenceResponse(false, 0, false, 0),
            new GetNotificationPreferenceResponse(true, 7), new GetGeneralPreferenceResponse(false));

        _createPreferenceResponse = new CreatePreferenceResponse(_id, _userId,
            new CreateFinancePreferenceResponse(false, 0, false, 0), new CreateNotificationPreferenceResponse(true, 7),
            new CreateGeneralPreferenceResponse(false));

        TestCandidate = new TestCandidate(_commandDispatcherMock.Object, _queryDispatcherMock.Object,
            MessageContextFactoryMock.Object);
    }
}