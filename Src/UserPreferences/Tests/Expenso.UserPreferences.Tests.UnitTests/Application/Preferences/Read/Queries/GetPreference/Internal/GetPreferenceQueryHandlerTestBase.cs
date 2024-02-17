using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.Internal.GetPreferenceQueryHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference.Internal;

internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Guid _id;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Mock<IUserContextAccessor> _userContextAccessorMock = null!;
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
        _userContextAccessorMock = new Mock<IUserContextAccessor>();
        _userContextMock = new Mock<IUserContext>();
        _userContextMock.SetupGet(x => x.UserId).Returns(_userId.ToString());
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object, _userContextAccessorMock.Object);
    }
}