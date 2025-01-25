using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByIdQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUserByIdQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByIdRequest(UserId: _userId));

        _userServiceMock
            .Setup(expression: x => x.GetUserByIdAsync(new GetUserByIdRequest(_userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByIdResponse);

        // Act
        GetUserByIdResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.ShouldNotBeNull();
        result.ShouldBeEquivalentTo(expected: _getUserByIdResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByIdRequest(UserId: _userId));

        _userServiceMock.Setup(expression: x =>
                x.GetUserByIdAsync(new GetUserByIdRequest(_userId), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        GetUserByIdResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.ShouldBeNull();
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object, Payload: null);

        _userServiceMock
            .Setup(expression: x => x.GetUserByIdAsync(null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(resourceName: "User",
                identifierType: IdentifierType.PrimaryId(), identifier: _userId));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"User with ID {_userId} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        exception.Identifier.ShouldBe(expected: _userId);
    }
}