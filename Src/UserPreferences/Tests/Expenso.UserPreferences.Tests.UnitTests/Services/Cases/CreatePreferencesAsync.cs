using Expenso.UserPreferences.Core.Application.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class CreatePreferencesAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_CreatePreferences()
    {
        // Arrange
        _preferencesRepositoryMock.Setup(x => x.CreateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(_preference);

        // Act
        PreferenceDto preference = await TestCandidate.CreatePreferencesAsync(_userId, default);

        // Assert
        preference.Should().NotBeNull();
        preference.PreferenceId.Should().Be(_preference.PreferenceId);
        preference.UserId.Should().Be(_preference.UserId);
        preference.FinancePreference.Should().BeEquivalentTo(_preference.FinancePreference);
        preference.NotificationPreference.Should().BeEquivalentTo(_preference.NotificationPreference);
        preference.GeneralPreference.Should().BeEquivalentTo(_preference.GeneralPreference);
        _preferencesRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}