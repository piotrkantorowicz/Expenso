using Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Queries.GetPreference;

internal abstract class GetPreferenceInternalQueryHandlerTestBase : TestBase<GetPreferenceInternalQueryHandler>
{
    protected GetPreferenceInternalResponse _getPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = Preference.CreateDefault(Guid.NewGuid(), _userId);
        _getPreferenceResponse = PreferenceMap.MapToInternalGetRequest(_preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new GetPreferenceInternalQueryHandler(_preferenceRepositoryMock.Object);
    }
}