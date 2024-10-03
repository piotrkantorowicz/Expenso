using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class GetByIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(sourceName: nameof(_preferenceIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid preferenceId)
    {
        // Arrange
        PreferenceFilter filter = new()
        {
            PreferenceIdOrUserId = preferenceId,
            UseTracking = false
        };

        // Act
        Preference? preference = await TestCandidate.GetAsync(preferenceFilter: filter, cancellationToken: default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(expected: Preferences.Single(predicate: x => x.Id == preferenceId));
    }
}