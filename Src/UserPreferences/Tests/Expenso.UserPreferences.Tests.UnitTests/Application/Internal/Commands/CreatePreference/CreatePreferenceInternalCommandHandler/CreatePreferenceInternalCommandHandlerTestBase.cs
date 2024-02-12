using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference.
    CreatePreferenceInternalCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Commands.CreatePreference.
    CreatePreferenceInternalCommandHandler;

internal abstract class CreatePreferenceInternalCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceInternalResponse _createPreferenceInternalResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected UserId _userId = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = UserId.New(Guid.NewGuid());
        _preference = Preference.CreateDefault(PreferenceId.New(Guid.NewGuid()), _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceInternalResponse = PreferenceMap.MapToInternalCreateResponse(_preference);
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}