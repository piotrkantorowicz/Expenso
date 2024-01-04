using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class CreatePreferencesAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task ShouldCreatePreferences()
    {
        // Arrange
        PreferencesRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(Preference);

        // Act
        PreferenceDto preference = await TestCandidate.CreatePreferencesAsync(UserId, default);

        // Assert
        preference.Should().NotBeNull();
        preference.PreferencesId.Should().Be(Preference.PreferencesId);
        preference.UserId.Should().Be(Preference.UserId);
        preference.FinancePreference.Should().BeEquivalentTo(Preference.FinancePreference);
        preference.NotificationPreference.Should().BeEquivalentTo(Preference.NotificationPreference);
        preference.GeneralPreference.Should().BeEquivalentTo(Preference.GeneralPreference);
        PreferencesRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}