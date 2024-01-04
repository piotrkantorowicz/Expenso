using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class CreatePreferencesInternalAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task ShouldCreatePreferences()
    {
        // Arrange
        PreferencesRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(Preference);

        // Act
        Guid preferenceId = await TestCandidate.CreatePreferencesInternalAsync(UserId, default);

        // Assert
        preferenceId.Should().Be(Preference.PreferencesId);
        PreferencesRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}