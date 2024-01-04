using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferencesForUserAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _preferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(_userId, false, default)).ReturnsAsync(_preference);

        // Act
        PreferenceDto preferenceDto = await TestCandidate.GetPreferencesForUserAsync(_userId, default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToDto(_preference));
        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(_userId, false, default), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        _preferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetPreferencesForUserAsync(_userId, default));

        exception
            ?.Message.Should()
            .Be(new StringBuilder()
                .Append("Preferences for user with id ")
                .Append(_userId)
                .Append(" not found.")
                .ToString());

        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(_userId, false, default), Times.Once);
    }
}