using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Mappings;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferencesForCurrentUserAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_PreferenceExists()
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
    public void Should_ThrowNotFoundException_When_PreferencesDoesNotExists()
    {
        // Arrange
        _userContextAccessorMock.Setup(x => x.Get()).Returns((IUserContext?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetPreferencesForCurrentUserAsync(default));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for current user not found, because user id from user context is empty.")
            .ToString();
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}