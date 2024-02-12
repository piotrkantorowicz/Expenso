using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Internal.Queries.GetPreference.
    GetPreferenceInternalQueryHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Queries.GetPreference.
    GetPreferenceInternalQueryHandler;

internal abstract class GetPreferenceInternalQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected GetPreferenceInternalResponse _getPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected UserId _userId = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = UserId.New(Guid.NewGuid());
        _preference = Preference.CreateDefault(PreferenceId.New(Guid.NewGuid()), _userId);
        _getPreferenceResponse = PreferenceMap.MapToInternalGetRequest(_preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}