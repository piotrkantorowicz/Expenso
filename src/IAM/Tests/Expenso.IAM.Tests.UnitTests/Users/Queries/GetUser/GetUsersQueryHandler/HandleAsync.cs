using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;
using Expenso.Shared.System.Types.Pagination;

using FluentAssertions;

using Moq;

using NUnit.Framework;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUsersQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUsersQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUsers_When_WithNoFilterAndUserExists()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Pagination: Paging.Default,
            Payload: new GetUsersRequest());

        GetUsersRequest getUsersRequest = new();

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(getUsersRequest, Paging.Default, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUsersResponse);

        // Act
        IPagedList? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUsersResponse);
    }

    [Test]
    public async Task Should_ThrowException_When_ServiceThrowsException()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Pagination: Paging.Default,
            Payload: new GetUsersRequest());

        _userServiceMock
            .Setup(expression: x =>
                x.GetUsersAsync(It.IsAny<GetUsersRequest>(), Paging.Default, It.IsAny<CancellationToken>()))
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
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Pagination: Paging.Default,
            Payload: new GetUsersRequest());

        GetUsersRequest getUsersRequest = new();

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(getUsersRequest, Paging.Default, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<GetUsersResponse>.AsEmpty);

        // Act
        IPagedList<GetUsersResponse>? getUsersResponse =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse?.Items.Should().BeEmpty();
    }

    [Test]
    public async Task Should_ReturnEmptyCollection_When_RequestIsNull()
    {
        // Arrange
        GetUsersQuery query = new(MessageContext: _messageContextMock.Object, Pagination: Paging.Default,
            Payload: null);

        _userServiceMock
            .Setup(expression: x => x.GetUsersAsync(null, Paging.Default, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: PagedList<GetUsersResponse>.AsEmpty);

        // Act
        IPagedList<GetUsersResponse>? getUsersResponse =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse?.Items.Should().BeEmpty();
    }
}