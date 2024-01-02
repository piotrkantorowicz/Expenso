using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class GetByIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(PreferenceIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid preferenceId)
    {
        // Arrange
        // Act
        Preference? preference = await TestCandidate.GetByIdAsync(preferenceId, false, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(Preferences.Single(x => x.PreferencesId == preferenceId));
    }
}