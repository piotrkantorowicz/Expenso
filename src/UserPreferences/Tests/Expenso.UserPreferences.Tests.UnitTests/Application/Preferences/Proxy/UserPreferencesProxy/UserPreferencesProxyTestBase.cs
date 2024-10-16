using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

using TestCandidate = Expenso.UserPreferences.Core.Application.Proxy.UserPreferencesProxy;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

[TestFixture]
internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _id = Guid.NewGuid();
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        _commandDispatcherMock = new Mock<ICommandDispatcher>();

        _getPreferencesExternalResponse = new GetPreferencesResponse(Id: _id, UserId: _userId,
            FinancePreference: new GetPreferencesResponse_FinancePreference(AllowAddFinancePlanSubOwners: false,
                MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: false,
                MaxNumberOfFinancePlanReviewers: 0),
            NotificationPreference: new GetPreferencesResponse_NotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 7),
            GeneralPreference: new GetPreferencesResponse_GeneralPreference(UseDarkMode: false));

        _createPreferenceResponse = new CreatePreferenceResponse(PreferenceId: _id);

        TestCandidate = new TestCandidate(commandDispatcher: _commandDispatcherMock.Object,
            queryDispatcher: _queryDispatcherMock.Object, messageContextFactory: MessageContextFactoryMock.Object);
    }

    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected GetPreferencesResponse _getPreferencesExternalResponse = null!;
    private Guid _id;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected Guid _userId;
}