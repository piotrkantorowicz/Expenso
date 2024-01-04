using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserPreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_ReturnPreferences_When_PreferencesExists()
    {
        // Arrange
        _preferenceServiceMock
            .Setup(x => x.GetPreferencesForUserInternalAsync(_userId, default))
            .ReturnsAsync(_preferenceContract);

        // Act
        PreferenceContract user = await TestCandidate.GetUserPreferencesAsync(_userId, default);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_preferenceContract);
        _preferenceServiceMock.Verify(x => x.GetPreferencesForUserInternalAsync(_userId, default), Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_PreferencesDoesNotExists()
    {
        // Arrange
        _preferenceServiceMock.Setup(x => x.GetPreferencesForUserInternalAsync(_userId, default))!.ReturnsAsync(
            (PreferenceContract?)null);

        // Act
        PreferenceContract user = await TestCandidate.GetUserPreferencesAsync(_userId, default);

        // Assert
        user.Should().BeNull();
        _preferenceServiceMock.Verify(x => x.GetPreferencesForUserInternalAsync(_userId, default), Times.Once);
    }
}