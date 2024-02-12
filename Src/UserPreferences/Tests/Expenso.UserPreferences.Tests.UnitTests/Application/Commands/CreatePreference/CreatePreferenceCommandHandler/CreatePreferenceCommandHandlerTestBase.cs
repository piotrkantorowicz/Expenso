using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Application.Preferences.Mappings;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

using TestCandidate =
    Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference.CreatePreferenceCommandHandler;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.CreatePreference.CreatePreferenceCommandHandler;

internal abstract class CreatePreferenceCommandHandlerTestBase : TestBase<TestCandidate>
{
    protected CreatePreferenceResponse _createPreferenceResponse = null!;
    protected Preference _preference = null!;
    protected Mock<IPreferencesRepository> _preferenceRepositoryMock = null!;
    protected UserId _userId = null!;

    [SetUp]
    public void SetUp()
    {
        _userId = UserId.New(Guid.NewGuid());
        _preference = Preference.CreateDefault(PreferenceId.New( Guid.NewGuid()), _userId);
        _preferenceRepositoryMock = new Mock<IPreferencesRepository>();
        _createPreferenceResponse = PreferenceMap.MapToCreateResponse(_preference);
        TestCandidate = new TestCandidate(_preferenceRepositoryMock.Object);
    }
}