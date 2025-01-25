using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Write.Commands.CreatePreference;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Request;
using Expenso.UserPreferences.Shared.DTO.API.CreatePreference.Response;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Write.CreatePreference.
    CreatePreferenceCommandHandler;

[TestFixture]
internal sealed class HandleAsync : CreatePreferenceCommandHandlerTestBase
{
    [Test]
    public async Task Should_ReturnCreatePreferenceResponse_When_CreatingPreference()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CreatePreferenceRequest(PreferenceId: _preferenceId, UserId: _userId));

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.ExistsAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: false);

        _preferenceRepositoryMock
            .Setup(expression: x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        CreatePreferenceResponse result =
            await TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expected: _createPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x => x.CreateAsync(It.IsAny<Preference>(), It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowConflictException_When_CreatingPreferenceWithSameIdAlreadyExists()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CreatePreferenceRequest(PreferenceId: _preferenceId, UserId: _userId));

        PreferenceQuerySpecification querySpecification = new()
        {
            PreferenceId = _preferenceId,
            UseTracking = false
        };

        _preferenceRepositoryMock
            .Setup(expression: x => x.ExistsAsync(querySpecification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        // Act
        // Assert
        Func<Task> action = () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        ConflictException? exception = await action.ShouldThrowAsync<ConflictException>();
        exception.Message.ShouldBe(expected: $"{nameof(Preference)} with query {querySpecification} already exists.");
        exception.ResourceName.ShouldBe(expected: nameof(Preference));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Query());
        ((PreferenceQuerySpecification?)exception.Identifier).ShouldBeEquivalentTo(expected: querySpecification);
    }
    
    [Test]
    public async Task Should_ThrowConflictException_When_CreatingPreferenceForUserAlreadyExists()
    {
        // Arrange
        CreatePreferenceCommand command = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new CreatePreferenceRequest(PreferenceId: _preferenceId, UserId: _userId));

        PreferenceQuerySpecification querySpecification = new()
        {
            UserId = _userId,
            UseTracking = false
        };

        _preferenceRepositoryMock
            .Setup(expression: x => x.ExistsAsync(querySpecification, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: true);

        // Act
        // Assert
        Func<Task> action = () =>
            TestCandidate.HandleAsync(command: command, cancellationToken: It.IsAny<CancellationToken>());

        ConflictException? exception = await action.ShouldThrowAsync<ConflictException>();
        exception.Message.ShouldBe(expected: $"{nameof(Preference)} with query {querySpecification} already exists.");
        exception.ResourceName.ShouldBe(expected: nameof(Preference));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Query());
        ((PreferenceQuerySpecification?)exception.Identifier).ShouldBeEquivalentTo(expected: querySpecification);
    }
}