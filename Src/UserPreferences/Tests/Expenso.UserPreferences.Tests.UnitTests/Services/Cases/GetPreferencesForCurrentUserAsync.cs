using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferencesForCurrentUserAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        UserContextAccessorMock.Setup(x => x.Get()).Returns(UserContextMock.Object);
        PreferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(UserId, false, default)).ReturnsAsync(Preference);

        // Act
        PreferenceDto preferenceDto = await TestCandidate.GetPreferencesForCurrentUserAsync(default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToDto(Preference));
        PreferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(UserId, false, default), Times.Once);
    }

    [Test]
    public void ShouldThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        UserContextAccessorMock.Setup(x => x.Get()).Returns((IUserContext?)null);

        PreferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(Guid.Empty, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetPreferencesForCurrentUserAsync(default));

        exception?.Message.Should().Be(new StringBuilder().Append("Preferences for user not found.").ToString());
        PreferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(Guid.Empty, false, default), Times.Once);
    }
}