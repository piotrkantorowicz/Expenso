using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Request;
using Expenso.UserPreferences.Core.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser.DTO.Response;
using Expenso.UserPreferences.Core.Domain.Preferences.Model;
using Expenso.UserPreferences.Core.Domain.Preferences.Repositories.Specifications;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.UserPreferences.Tests.UnitTests.Application.Preferences.Read.Queries.GetPreferenceForCurrentUser;

[TestFixture]
internal sealed class HandleAsync : GetPreferenceForCurrentUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnPreference_When_PreferenceExists()
    {
        // Arrange
        GetPreferenceForCurrentUserQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceForCurrentUserRequest(
                Includes: It.IsAny<GetPreferenceForCurrentUserRequestPreferenceIncludes>()));

        _userContextAccessorMock.Setup(expression: x => x.Get()).Returns(value: _executionContextMock.Object);

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _preference);

        // Act
        GetPreferenceForCurrentUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expected: _getPreferenceResponse);

        _preferenceRepositoryMock.Verify(
            expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()), times: Times.Once);

        _userContextAccessorMock.Verify(expression: x => x.Get(), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_PreferenceHasNotBeenFound()
    {
        // Arrange
        GetPreferenceForCurrentUserQuery query = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Payload: new GetPreferenceForCurrentUserRequest(
                Includes: It.IsAny<GetPreferenceForCurrentUserRequestPreferenceIncludes>()));

        _userContextAccessorMock.Setup(expression: x => x.Get()).Returns(value: _executionContextMock.Object);

        _preferenceRepositoryMock
            .Setup(expression: x =>
                x.GetAsync(new PreferenceQuerySpecification(null, _userId, false, It.IsAny<PreferenceIncludes>()),
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        PreferenceQuerySpecification preferenceQuerySpecification = new(UserId: _userId, UseTracking: false,
            Includes: It.IsAny<PreferenceIncludes>());

        // Act
        Func<Task> action = () =>
            TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException exception = await action.ShouldThrowAsync<NotFoundException>();

        exception.Message.ShouldBe(
            expected: $"{nameof(Preference)} with query {preferenceQuerySpecification} hasn't been found.");

        exception.ResourceName.ShouldBe(expected: nameof(Preference));
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Query());

        ((PreferenceQuerySpecification?)exception.Identifier).ShouldBeEquivalentTo(
            expected: preferenceQuerySpecification);
    }
}