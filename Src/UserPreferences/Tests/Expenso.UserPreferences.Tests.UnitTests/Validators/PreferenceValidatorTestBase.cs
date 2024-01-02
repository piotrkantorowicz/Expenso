using Expenso.UserPreferences.Core.Repositories;
using Expenso.UserPreferences.Core.Validators;

namespace Expenso.UserPreferences.Tests.UnitTests.Validators;

internal abstract class PreferenceValidatorTestBase : TestBase
{
    protected PreferenceValidator TestCandidate { get; private set; } = null!;

    protected Mock<IPreferencesRepository> PreferencesRepositoryMock { get; private set; } = null!;

    [SetUp]
    public void SetUp()
    {
        PreferencesRepositoryMock = new Mock<IPreferencesRepository>();
        TestCandidate = new PreferenceValidator(PreferencesRepositoryMock.Object);
    }
}