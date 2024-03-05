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
            .Setup(x => x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == _userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getUserResponse);

        // Act
        GetUserResponse? getUserResponse = await TestCandidate.GetUserByIdAsync(_userId, It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.Should().NotBeNull();
        getUserResponse.Should().BeEquivalentTo(_getUserResponse);

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == _userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _queryDispatcherMock
            .Setup(x => x.QueryAsync(It.Is<GetUserQuery>(y => y.UserId == userId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"User with id {userId} not found."));

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() =>
            TestCandidate.GetUserByIdAsync(userId, It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"User with id {userId} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}