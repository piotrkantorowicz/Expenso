using Expenso.UserPreferences.Core.Application.Validators;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators;

internal abstract class PreferenceValidatorTestBase : TestBase<IPreferenceValidator>
{
    protected Mock<IPreferencesRepository> _preferencesRepositoryMock = null!;

    [SetUp]
    public void SetUp()
    {
        _preferencesRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new PreferenceValidator(_preferencesRepositoryMock.Object);
    }
}