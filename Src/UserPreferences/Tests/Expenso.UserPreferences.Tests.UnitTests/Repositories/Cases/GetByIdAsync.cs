using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class GetByIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(_preferenceIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(PreferenceId preferenceId)
    {
        // Arrange
        // Act
        Preference? preference = await TestCandidate.GetByIdAsync(preferenceId, false, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(Preferences.Single(x => x.PreferenceId == preferenceId));
    }
}