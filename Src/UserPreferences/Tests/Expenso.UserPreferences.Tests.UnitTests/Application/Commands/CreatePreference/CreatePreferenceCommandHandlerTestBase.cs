using Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.CreatePreference;

internal abstract class CreatePreferenceCommandHandlerTestBase : TestBase<CreatePreferenceCommandHandler>
{
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;
    protected CreatePreferenceResponse createPreferenceResponse = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = Preference.CreateDefault(Guid.NewGuid(), _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        createPreferenceResponse = PreferenceMap.MapToCreateResponse(_preference);
        TestCandidate = new CreatePreferenceCommandHandler(_preferenceRepositoryMock.Object);
    }
}