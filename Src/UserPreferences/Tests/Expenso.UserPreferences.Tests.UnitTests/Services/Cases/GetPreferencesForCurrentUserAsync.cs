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
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _userContextAccessorMock.Setup(x => x.Get()).Returns(_userContextMock.Object);
        _preferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(_userId, false, default)).ReturnsAsync(_preference);

        // Act
        PreferenceDto preferenceDto = await TestCandidate.GetPreferencesForCurrentUserAsync(default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToDto(_preference));
        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(_userId, false, default), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        _userContextAccessorMock.Setup(x => x.Get()).Returns((IUserContext?)null);

        _preferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(Guid.Empty, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetPreferencesForCurrentUserAsync(default));

        exception?.Message.Should().Be(new StringBuilder().Append("Preferences for user not found.").ToString());
        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(Guid.Empty, false, default), Times.Once);
    }
}