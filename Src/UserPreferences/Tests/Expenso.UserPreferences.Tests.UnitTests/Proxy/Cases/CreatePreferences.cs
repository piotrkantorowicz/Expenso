namespace Expenso.UserPreferences.Tests.UnitTests.Proxy.Cases;

internal sealed class CreatePreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_CallCreatePreference()
    {
        // Arrange
        // Act
        await TestCandidate.CreatePreferencesAsync(UserId, default);

        // Assert
        PreferenceServiceMock.Verify(x => x.CreatePreferencesInternalAsync(UserId, default), Times.Once);
    }
}