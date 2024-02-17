using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.GetPreference.Request;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreference.External.GetPreferenceQueryHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreference.External;

internal abstract class GetPreferenceQueryHandlerTestBase : TestBase<TestCandidate>
{
    protected GetPreferenceResponse _getPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(_userId);
        _getPreferenceResponse = GetPreferenceResponseMap.MapTo(_preference);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}