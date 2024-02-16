using System.Text;

using Expenso.Shared.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference.External;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.External.
    CreatePreferenceCommandHandler;

internal sealed class HandleAsync : CreatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_ReturnCreatePreferenceResponse_When_CreatingPreference()
    {
        // Arrange
        CreatePreferenceCommand command = new(new CreatePreferenceRequest(_userId.Value));

        _preferenceRepositoryMock
            .Setup(x => x.ExistsAsync(new PreferenceFilter(null, _userId, false, null, null, null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _preferenceRepositoryMock
            .Setup(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_preference);

        // Act
        CreatePreferenceResponse? result = await TestCandidate.HandleAsync(command);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_createPreferenceResponse);

        _preferenceRepositoryMock.Verify(x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        CreatePreferenceCommand command = new(new CreatePreferenceRequest(_userId.Value));

        _preferenceRepositoryMock
            .Setup(x => x.ExistsAsync(new PreferenceFilter(null, _userId, false, null, null, null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        // Assert
        ConflictException? exception = Assert.ThrowsAsync<ConflictException>(() => TestCandidate.HandleAsync(command));

        string expectedExceptionMessage = new StringBuilder()
            .Append("Preferences for user with id ")
            .Append(command.Preference.UserId)
            .Append(" already exists")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}