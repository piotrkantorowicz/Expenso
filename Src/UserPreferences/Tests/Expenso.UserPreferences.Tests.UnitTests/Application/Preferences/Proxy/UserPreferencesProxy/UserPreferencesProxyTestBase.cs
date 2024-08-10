using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Proxy;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

using TestCandidate = Expenso.UserPreferences.Core.Application.Proxy.UserPreferencesProxy;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected GetPreferenceResponse _getPreferenceExternalResponse = null!;
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

        _getPreferenceExternalResponse = new GetPreferenceResponse(Id: _id, UserId: _userId,
            FinancePreference: new GetPreferenceResponse_FinancePreference(AllowAddFinancePlanSubOwners: false,
                MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: false,
                MaxNumberOfFinancePlanReviewers: 0),
            NotificationPreference: new GetPreferenceResponse_NotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 7),
            GeneralPreference: new GetPreferenceResponse_GeneralPreference(UseDarkMode: false));

        _createPreferenceResponse = new CreatePreferenceResponse(PreferenceId: _id);

        TestCandidate = new TestCandidate(commandDispatcher: _commandDispatcherMock.Object,
            queryDispatcher: _queryDispatcherMock.Object, messageContextFactory: MessageContextFactoryMock.Object);
    }
}