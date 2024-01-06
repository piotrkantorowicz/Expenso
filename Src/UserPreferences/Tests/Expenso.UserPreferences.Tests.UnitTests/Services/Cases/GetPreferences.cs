using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.Shared.UserContext;
using Expenso.UserPreferences.Core.DTO.GetUserPreferences;
using Expenso.UserPreferences.Core.Mappings;
using Expenso.UserPreferences.Core.Models;

namespace Expenso.UserPreferences.Tests.UnitTests.Services.Cases;

internal sealed class GetPreferences : PreferenceServiceTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_PreferenceExists()
    {
        // Arrange
        _userContextAccessorMock.Setup(x => x.Get()).Returns(_userContextMock.Object);
        _preferencesRepositoryMock.Setup(x => x.GetByIdAsync(_preferenceId, false, default)).ReturnsAsync(_preference);

        // Act
        PreferenceDto preferenceDto = await TestCandidate.GetPreferences(_preferenceId, default);

        // Assert
        preferenceDto.Should().NotBeNull();
        preferenceDto.Should().BeEquivalentTo(PreferenceMap.MapToDto(_preference));
        _preferencesRepositoryMock.Verify(x => x.GetByIdAsync(_preferenceId, false, default), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_PreferencesDoesNotExists()
    {
        // Arrange
        _userContextAccessorMock.Setup(x => x.Get()).Returns((IUserContext?)null);

        _preferencesRepositoryMock
            .Setup(x => x.GetByIdAsync(_preferenceId, false, default))
            .ReturnsAsync((Preference?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetPreferences(_preferenceId, default));

        exception
            ?.Message.Should()
            .Be(new StringBuilder()
                .Append("Preferences with id ")
                .Append(_preferenceId)
                .Append(" not found.")
                .ToString());

        _preferencesRepositoryMock.Verify(x => x.GetByIdAsync(_preferenceId, false, default), Times.Once);
    }
}