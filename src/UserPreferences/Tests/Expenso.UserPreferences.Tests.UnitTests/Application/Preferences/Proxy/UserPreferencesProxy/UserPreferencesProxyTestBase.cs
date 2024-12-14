using Expenso.Shared.Commands.Dispatchers;
using Expenso.Shared.Queries.Dispatchers;
using Expenso.Shared.System.Modules.Constants;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.UserPreferences.Shared;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Proxy.UserPreferencesProxy;

[TestFixture]
internal abstract class UserPreferencesProxyTestBase : TestBase<IUserPreferencesProxy>
{
    [SetUp]
    public void SetUp()
    {
        _preferenceId = Guid.NewGuid();
        _userId = Guid.NewGuid();
        _id = Guid.NewGuid();
        _queryDispatcherMock = new Mock<IQueryDispatcher>();
        _commandDispatcherMock = new Mock<ICommandDispatcher>();

        _currentMessageContext = MessageContextFactoryMock.Object.Current(messageId: It.IsAny<Guid?>(),
            moduleId: ModuleNames.UserPreferencesModule);

        _getPreferencesExternalResponse = new GetPreferencesResponse(Id: _id, UserId: _userId,
            FinancePreference: new GetPreferencesResponseFinancePreference(AllowAddFinancePlanSubOwners: false,
                MaxNumberOfSubFinancePlanSubOwners: 0, AllowAddFinancePlanReviewers: false,
                MaxNumberOfFinancePlanReviewers: 0),
            NotificationPreference: new GetPreferencesResponseNotificationPreference(SendFinanceReportEnabled: true,
                SendFinanceReportInterval: 7),
            GeneralPreference: new GetPreferencesResponseGeneralPreference(UseDarkMode: false));

        _createPreferenceResponse = new CreatePreferenceResponse(PreferenceId: _id);

        TestCandidate = new Core.Application.Proxy.UserPreferencesProxy(
            commandDispatcher: _commandDispatcherMock.Object, queryDispatcher: _queryDispatcherMock.Object,
            messageContextFactory: MessageContextFactoryMock.Object);
    }

    protected IMessageContext _currentMessageContext = null!;
    protected Mock<ICommandDispatcher> _commandDispatcherMock = null!;
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected GetPreferencesResponse _getPreferencesExternalResponse = null!;
    private Guid _id;
    protected Mock<IQueryDispatcher> _queryDispatcherMock = null!;
    protected Guid _userId;
    protected Guid _preferenceId;
}