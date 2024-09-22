using Expenso.Shared.System.Types.Exceptions;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Filters;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Proxy.DTO.API.CreatePreference.Response;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandHandler;

internal sealed class HandleAsync : CreatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_ReturnCreatePreferenceResponse_When_CreatingPreference()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Preference: new CreatePreferenceRequest(UserId: _userId));

        _preferenceRepositoryMock
            .Setup(expression: x => x.ExistsAsync(new PreferenceFilter(null, _userId, false, null, null, null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: false);

        _preferenceRepositoryMock
            .Setup(expression: x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        CreatePreferenceResponse? result =
            await TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _createPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public void Should_ThrowConflictException_When_CreatingPreferenceAndPreferenceAlreadyExists()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Preference: new CreatePreferenceRequest(UserId: _userId));

        _preferenceRepositoryMock
            .Setup(expression: x => x.ExistsAsync(new PreferenceFilter(null, _userId, false, null, null, null),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        // Act
        // Assert
        Func<Task> act = () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        act
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(
                expectedWildcardPattern: $"Preferences for user with id {command.Preference.UserId} already exists.");
    }
}