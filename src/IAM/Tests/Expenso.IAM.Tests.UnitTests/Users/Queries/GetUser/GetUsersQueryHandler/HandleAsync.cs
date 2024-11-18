using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUsersQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUsersQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUsers_When_WithNoFilterAndUserExists()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Payload: new GetUsersRequest());
        GetUsersRequest getUsersRequest = new();

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(getUsersRequest, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByIdResponse);

        // Act
        IReadOnlyCollection<GetUsersResponse>? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserByIdResponse);
    }

    [Test]
    public async Task Should_ThrowException_When_ServiceThrowsException()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Payload: new GetUsersRequest());

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<GetUsersRequest>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Service error"));

        // Act & Assert
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        await action.Should().ThrowAsync<Exception>().WithMessage(expectedWildcardPattern: "Service error");
    }

    [Test]
    public async Task Should_ReturnEmptyCollection_When_UsersHasNotBeenFound()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Payload: new GetUsersRequest());
        GetUsersRequest getUsersRequest = new();

        _userServiceMock.Setup(expression: x => x.GetUsersAsync(getUsersRequest, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: []);

        // Act
        IReadOnlyCollection<GetUsersResponse>? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnEmptyCollection_When_RequestIsNull()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Payload: null);

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: []);

        // Act
        IReadOnlyCollection<GetUsersResponse>? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeEmpty();
    }
}