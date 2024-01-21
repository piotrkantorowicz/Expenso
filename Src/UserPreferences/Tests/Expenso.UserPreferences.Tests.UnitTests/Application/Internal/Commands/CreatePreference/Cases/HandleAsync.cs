using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Internal.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Internal.Commands.CreatePreference.Cases;

internal sealed class HandleAsync : CreatePreferenceInternalCommandHandlerTestBase
{
    [Test]
    public async Task Should_ReturnCreatePreferenceResponse_When_CreatingPreference()
    {
        // Arrange
        CreatePreferenceInternalCommand command = new(new CreatePreferenceInternalRequest(_userId));

        _preferenceRepositoryMock
            .Setup(x => x.GetByUserIdAsync(_userId, It.IsAny<bool>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Preference?)null);

        _preferenceRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        CreatePreferenceInternalResponse? result = await TestCandidate.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_createPreferenceInternalResponse);

        _preferenceRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        CreatePreferenceInternalCommand command = new(new CreatePreferenceInternalRequest(_userId));

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