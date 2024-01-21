using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Core.Application.Preferences.Proxy;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Proxy;

internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected CreatePreferenceInternalResponse _createPreferenceInternalResponse = null!;
    protected GetPreferenceInternalResponse _getPreferenceInternalResponse = null!;
    protected Guid _id;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _id = Guid.NewGuid();
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        _commandDispatcherMock = new Mock<ICommandDispatcher>();

        _getPreferenceInternalResponse = new GetPreferenceInternalResponse(
            new GetFinancePreferenceInternalResponse(false, 0, false, 0),
            new GetNotificationPreferenceInternalResponse(true, 7), new GetGeneralPreferenceInternalResponse(false));

        _createPreferenceInternalResponse = new CreatePreferenceInternalResponse(_id, _userId,
            new CreateFinancePreferenceInternalResponse(false, 0, false, 0),
            new CreateNotificationPreferenceInternalResponse(true, 7),
            new CreateGeneralPreferenceInternalResponse(false));

        TestCandidate = new UserPreferencesProxy(_commandDispatcherMock.Object, _queryDispatcherMock.Object);
    }
}