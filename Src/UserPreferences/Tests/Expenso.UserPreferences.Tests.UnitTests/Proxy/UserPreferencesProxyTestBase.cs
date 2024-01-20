using Expenso.Shared.Commands;
using Expenso.Shared.Queries;
using Expenso.UserPreferences.Core.Application.Preferences.Proxy;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Proxy;

internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected GetPreferenceInternalResponse _getPreferenceInternalResponse = null!;
    protected CreatePreferenceInternalResponse _createPreferenceInternalResponse = null!;
    protected Guid _userId;
    protected Guid _id;

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