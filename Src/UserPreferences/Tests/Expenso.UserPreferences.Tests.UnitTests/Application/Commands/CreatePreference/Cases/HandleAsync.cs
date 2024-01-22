using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Request;
using Expenso.UserPreferences.Core.Application.Preferences.DTO.CreatePreference.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Commands.CreatePreference.Cases;

internal sealed class HandleAsync : CreatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_ReturnCreatePreferenceResponse_When_CreatingPreference()
    {
        // Arrange
        CreatePreferenceCommand command = new(new CreatePreferenceRequest(_userId));

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Preference?)null);

        _preferenceRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        CreatePreferenceResponse? result = await TestCandidate.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(createPreferenceResponse);

        _preferenceRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        CreatePreferenceCommand command = new(new CreatePreferenceRequest(_userId));

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(() => TestCandidate.HandleAsync(command));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(command.Preference.UserId)
            .Append(" already exists.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}