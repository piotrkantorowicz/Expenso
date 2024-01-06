using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;
using Expenso.UserPreferences.Proxy.Contracts.GetUserPreferences;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferencesForUserInternalAsync : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_PreferenceExists()
    {
        // Arrange
        _preferencesRepositoryMock.Setup(x => x.GetByUserIdAsync(_userId, false, default)).ReturnsAsync(_preference);

        // Act
        PreferenceContract preferenceDto = await TestCandidate.GetPreferencesForUserInternalAsync(_userId, default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToContract(_preference));
        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(_userId, false, default), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_PreferencesDoesNotExists()
    {
        // Arrange
        _preferencesRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() =>
                TestCandidate.GetPreferencesForUserInternalAsync(_userId, default));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(_userId)
            .Append(" not found.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);

        _preferencesRepositoryMock.Verify(x => x.GetByUserIdAsync(_userId, false, default), Times.Once);
    }
}