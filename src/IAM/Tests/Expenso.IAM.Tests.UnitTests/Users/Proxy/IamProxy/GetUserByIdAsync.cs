using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == _userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        // Act
        GetUserResponse? getUserResponse =
            await TestCandidate.GetUserByIdAsync(userId: _userId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.Should().NotBeNull();
        getUserResponse.Should().BeEquivalentTo(expectation: _getUserResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == _userId), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == userId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(message: $"User with id {userId} not found"));

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.GetUserByIdAsync(userId: userId, cancellationToken: It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"User with id {userId} not found";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}