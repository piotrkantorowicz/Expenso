using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class GetByUserIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(UserIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(Guid userId)
    {
        // Arrange
        // Act
        Preference? preference = await TestCandidate.GetByUserIdAsync(userId, false, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(Preferences.Single(x => x.UserId == userId));
    }
}