using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference;

[TestFixture]
internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<GetPreferenceQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _preference = PreferenceFactory.Create(userId: Guid.NewGuid());
        _getPreferenceResponse = GetPreferenceResponseMap.MapTo(preference: _preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new GetPreferenceQueryHandler(preferencesRepository: _preferenceRepositoryMock.Object);
    }

    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Guid _id;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
}