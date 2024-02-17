using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Maps;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.DTO.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Shared.Factories;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.Internal.CreatePreferenceCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.Internal.
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