using Expenso.UserPreferences.Core.DTO.UpdateUserPreferences;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class UpdatePreferencesAsync : PreferenceServiceTestBase
{
    private readonly UpdatePreferenceDto _updatePreferenceDto = new(new UpdateFinancePreferenceDto(true, 2, true, 5),
        new UpdateNotificationPreferenceDto(true, 3), new UpdateGeneralPreferenceDto(false));

    [Test]
    public async Task Should_UpdatePreferences()
    {
        // Arrange
        _preferencesRepositoryMock.Setup(x => x.UpdateAsync(It.IsAny<Preference>(), default)).ReturnsAsync(_preference);

        _preferenceValidatorMock
            .Setup(x => x.ValidateUpdateAsync(_userId, _updatePreferenceDto, default))
            .ReturnsAsync(_preference);

        // Act
        await TestCandidate.UpdatePreferencesAsync(_userId, _updatePreferenceDto, default);

        // Assert
        _preference.FinancePreference.Should().BeEquivalentTo(_updatePreferenceDto.FinancePreference);
        _preference.NotificationPreference.Should().BeEquivalentTo(_updatePreferenceDto.NotificationPreference);
        _preference.GeneralPreference.Should().BeEquivalentTo(_updatePreferenceDto.GeneralPreference);
        _preferencesRepositoryMock.Verify(x => x.UpdateAsync(It.IsAny<Preference>(), default), Times.Once);
    }
}