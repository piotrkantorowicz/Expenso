using Expenso.Shared.System.Types.ExecutionContext;
using Expenso.Shared.System.Types.ExecutionContext.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<GetPreferencesQueryHandler>
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
        _preference = PreferenceFactory.Create(userId: _userId);
        _getPreferenceResponse = GetPreferenceResponseMap.MapTo(preference: _preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _userContextAccessorMock = new Mock<IExecutionContextAccessor>();
        _userContextMock = new Mock<IUserContext>();
        _executionContextMock = new Mock<IExecutionContext>();
        _userContextMock.SetupGet(expression: x => x.UserId).Returns(value: _userId.ToString());
        _executionContextMock.SetupGet(expression: x => x.UserContext).Returns(value: _userContextMock.Object);

        TestCandidate = new GetPreferencesQueryHandler(preferencesRepository: _preferenceRepositoryMock.Object,
            executionContextAccessor: _userContextAccessorMock.Object);
    }
}