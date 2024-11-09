using Expenso.IAM.Core.Application.Users.Read.Queries.GetUsers;
using Expenso.IAM.Shared.DTO.GetUsers.Request;
using Expenso.IAM.Shared.DTO.GetUsers.Response;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal sealed class GetUsersAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUsersResponse);

        // Act
        IReadOnlyCollection<GetUsersResponse>? getUsersResponse = await TestCandidate.GetUsersAsync(
            request: new GetUsersRequest(), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse.Should().NotBeNull();
        getUsersResponse.Should().BeEquivalentTo(expectation: _getUsersResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.IsAny<GetUsersQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: null);

        // Act
        IReadOnlyCollection<GetUsersResponse>? getUsersResponse = await TestCandidate.GetUsersAsync(
            request: new GetUsersRequest(), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUsersResponse.Should().BeNull();
    }
}