using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Commands.CreatePreference;

internal abstract class
    CreatePreferenceInternalCommandHandlerTestBase : TestBase<CreatePreferenceInternalCommandHandler>
{
    protected CreatePreferenceInternalResponse _createPreferenceInternalResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected Guid _userId;

    [SetUp]
    public void SetUp()
    {
        _userId = Guid.NewGuid();
        _preference = Preference.CreateDefault(Guid.NewGuid(), _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceInternalResponse = PreferenceMap.MapToInternalCreateResponse(_preference);
        TestCandidate = new CreatePreferenceInternalCommandHandler(_preferenceRepositoryMock.Object);
    }
}