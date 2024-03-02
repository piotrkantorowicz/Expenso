using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.GetPreferenceQueryHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected Mock<IExecutionContext> _executionContextMock = null!;
    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Guid _id;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Mock<IExecutionContextAccessor> _userContextAccessorMock = null!;
    protected Mock<IUserContext> _userContextMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _userId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(_userId);
        _getPreferenceResponse = GetPreferenceResponseMap.MapTo(_preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _userContextAccessorMock = new Mock<IExecutionContextAccessor>();
        _userContextMock = new Mock<IUserContext>();
        _executionContextMock = new Mock<IExecutionContext>();
        _userContextMock.SetupGet(x => x.UserId).Returns(_userId.ToString());
        _executionContextMock.SetupGet(x => x.UserContext).Returns(_userContextMock.Object);
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object, _userContextAccessorMock.Object);
    }
}