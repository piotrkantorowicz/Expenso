using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferences.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Shared.DTO.API.GetPreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreferences;

[TestFixture]
internal abstract class GetPreferencesQueryHandlerTestBase : TestBase<GetPreferencesQueryHandler>
{
    [SetUp]
    public void SetUp()
    {
        _id = Guid.NewGuid();
        _userId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(userId: _userId);
        _getPreferenceResponse = GetPreferencesResponseMap.MapTo(preference: _preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new GetPreferencesQueryHandler(preferencesRepository: _preferenceRepositoryMock.Object);
    }

    protected Guid _id;
    protected Guid _userId;
    protected GetPreferencesResponse _getPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
}