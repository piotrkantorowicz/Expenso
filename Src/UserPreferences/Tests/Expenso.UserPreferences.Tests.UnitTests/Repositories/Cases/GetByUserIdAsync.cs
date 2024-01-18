using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Model.ValueObjects;

namespace Expenso.UserPreferences.Tests.UnitTests.Repositories.Cases;

internal sealed class GetByUserIdAsync : PreferenceRepositoryTestBase
{
    [Test, TestCaseSource(nameof(_userIds))]
    public async Task Should_ReturnPreference_When_PreferenceExists(UserId userId)
    {
        // Arrange
        // Act
        Preference? preference = await TestCandidate.GetByUserIdAsync(userId, false, default);

        // Assert
        preference.Should().NotBeNull();
        preference.Should().Be(Preferences.Single(x => x.UserId == userId));
    }
}