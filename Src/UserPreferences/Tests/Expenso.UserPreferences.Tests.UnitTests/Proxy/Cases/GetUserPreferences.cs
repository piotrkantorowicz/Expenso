using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task ShouldReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        PreferenceServiceMock
            .Setup(x => x.GetPreferencesForUserInternalAsync(UserId, default))
            .ReturnsAsync(PreferenceContract);

        // Act
        PreferenceContract user = await TestCandidate.GetUserPreferencesAsync(UserId, default);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(PreferenceContract);
        PreferenceServiceMock.Verify(x => x.GetPreferencesForUserInternalAsync(UserId, default), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        Guid userId = Guid.NewGuid();

        PreferenceServiceMock.Setup(x => x.GetPreferencesForUserInternalAsync(userId, default))!.ReturnsAsync(
            (PreferenceContract?)null);

        // Act
        PreferenceContract user = await TestCandidate.GetUserPreferencesAsync(userId, default);

        // Assert
        user.Should().BeNull();
        PreferenceServiceMock.Verify(x => x.GetPreferencesForUserInternalAsync(userId, default), Times.Once);
    }
}