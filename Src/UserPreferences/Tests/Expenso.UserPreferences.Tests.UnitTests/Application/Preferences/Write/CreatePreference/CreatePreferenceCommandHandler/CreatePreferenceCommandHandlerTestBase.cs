using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.CreatePreferenceCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandHandler;

internal abstract class CreatePreferenceCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = PreferenceFactory.Create(_userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceResponse = CreatePreferenceResponseMap.MapTo(_preference);
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}