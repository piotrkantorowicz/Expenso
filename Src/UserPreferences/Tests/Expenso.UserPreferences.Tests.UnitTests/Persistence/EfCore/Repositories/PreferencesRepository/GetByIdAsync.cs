using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class GetByIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(_preferenceIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid preferenceId)
    {
        // Arrange
        PreferenceFilter filter = new()
        {
            Id = preferenceId,
            UseTracking = false
        };

        // Act
        Preference? preference = await TestCandidate.GetAsync(filter, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(Preferences.Single(x => x.Id == preferenceId));
    }
}