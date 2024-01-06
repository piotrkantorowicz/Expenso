namespace Expenso.UserPreferences.Tests.UnitTests.Proxy.Cases;

internal sealed class CreatePreferences : UserPreferencesProxyTestBase
{
    [Test]
    public async Task Should_CallCreatePreference()
    {
        // Arrange
        // Act
        await TestCandidate.CreatePreferencesAsync(_userId, default);

        // Assert
        _preferenceServiceMock.Verify(x => x.CreatePreferencesInternalAsync(_userId, default), Times.Once);
    }
}