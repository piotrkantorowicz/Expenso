using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByEmailQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUserByEmailQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByEmailRequest(Email: _userEmail));

        _userServiceMock
            .Setup(expression: x =>
                x.GetUserByEmailAsync(new GetUserByEmailRequest(_userEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        // Act
        GetUserByEmailResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserByEmailResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByEmailRequest(Email: _userEmail));

        _userServiceMock.Setup(expression: x =>
                x.GetUserByEmailAsync(new GetUserByEmailRequest(_userEmail), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        GetUserByEmailResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object, Payload: null);

        _userServiceMock
            .Setup(expression: x => x.GetUserByEmailAsync(null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(resourceName: "User", identifierType: IdentifierType.Email(),
                identifier: _userEmail));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with email {_userEmail} hasn't been found.")
            .Where(exceptionExpression: x => x.ResourceName == "User" && x.IdentifierType == IdentifierType.Email() &&
                                             (string?)x.Identifier == _userEmail);
    }
}