using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.GetPreferences.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Queries.GetPreference.GetPreferenceQueryHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Queries.GetPreference.GetPreferenceQueryHandler;

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
        _preference = Preference.CreateDefault(_id, _userId);
        _getPreferenceResponse = PreferenceMap.MapToGetResponse(_preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _userContextAccessorMock = new Mock<IUserContextAccessor>();
        _userContextMock = new Mock<IUserContext>();
        _userContextMock.SetupGet(x => x.UserId).Returns(_userId.ToString());
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object, _userContextAccessorMock.Object);
    }
}