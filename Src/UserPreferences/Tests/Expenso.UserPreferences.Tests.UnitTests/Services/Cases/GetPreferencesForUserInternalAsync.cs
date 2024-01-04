using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferencesForUserInternalAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        PreferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(UserId, false, default)).ReturnsAsync(Preference);

        // Act
        PreferenceContract preferenceDto = await TestCandidate.GetPreferencesForUserInternalAsync(UserId, default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToContract(Preference));
        PreferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(UserId, false, default), Times.Once);
    }

    [Test]
    public void ShouldThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        PreferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(UserId, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() =>
                TestCandidate.GetPreferencesForUserInternalAsync(UserId, default));

        exception
            ?.Message.Should()
            .Be(new StringBuilder()
                .Append("Preferences for user with id ")
                .Append(UserId)
                .Append(" not found.")
                .ToString());

        PreferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(UserId, false, default), Times.Once);
    }
}