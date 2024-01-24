using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference.CreatePreferenceCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.CreatePreference.CreatePreferenceCommandHandler;

internal abstract class CreatePreferenceCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;
    protected CreatePreferenceResponse _createPreferenceResponse = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = Preference.CreateDefault(Guid.NewGuid(), _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceResponse = PreferenceMap.MapToCreateResponse(_preference);
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}
