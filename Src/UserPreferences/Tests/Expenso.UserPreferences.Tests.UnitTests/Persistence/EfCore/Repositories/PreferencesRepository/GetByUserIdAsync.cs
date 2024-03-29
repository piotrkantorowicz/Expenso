using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;

namespace Expenso.UserPreferences.Tests.UnitTests.Persistence.EfCore.Repositories.PreferencesRepository;

internal sealed class GetByUserIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(_userIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid userId)
    {
        // Arrange
        PreferenceFilter filter = new()
        {
            UserId = userId,
            UseTracking = false
        };

        // Act
        Preference? preference = await TestCandidate.GetAsync(filter, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(_preferenceDbSetMock.Object.Single(x => x.UserId == userId));
    }
}