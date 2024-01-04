using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class CreatePreferencesInternalAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_CreatePreferences()
    {
        // Arrange
        _preferencesRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(_preference);

        // Act
        Guid preferenceId = await TestCandidate.CreatePreferencesInternalAsync(_userId, default);

        // Assert
        preferenceId.Should().Be(_preference.PreferencesId);
        _preferencesRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}