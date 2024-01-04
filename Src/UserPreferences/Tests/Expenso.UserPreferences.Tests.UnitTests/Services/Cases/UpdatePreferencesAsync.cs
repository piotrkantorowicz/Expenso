using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class UpdatePreferencesAsync : PreferenceServiceTestBase
{
    private readonly UpdatePreferenceDto _updatePreferenceDto = new(new UpdateFinancePreferenceDto(true, 2, true, 5),
        new UpdateNotificationPreferenceDto(true, 3), new UpdateGeneralPreferenceDto(false));

    [Test]
    public async Task ShouldUpdatePreferences()
    {
        // Arrange
        PreferencesRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(Preference);

        PreferenceValidatorMock
            .Setup(x => x.ValidateUpdateAsync(UserId, _updatePreferenceDto, default))
            .ReturnsAsync(Preference);

        // Act
        await TestCandidate.UpdatePreferencesAsync(UserId, _updatePreferenceDto, default);

        // Assert
        Preference.FinancePreference.Should().BeEquivalentTo(_updatePreferenceDto.FinancePreference);
        Preference.NotificationPreference.Should().BeEquivalentTo(_updatePreferenceDto.NotificationPreference);
        Preference.GeneralPreference.Should().BeEquivalentTo(_updatePreferenceDto.GeneralPreference);
        PreferencesRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}