using Expenso.IAM.Core.Users.Queries.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getUserResponse);

        // Act
        GetUserExternalResponse? getUserInternal =
            await TestCandidate.GetUserByEmailAsync(_userEmail, It.IsAny<CancellationToken>());

        // Assert
        getUserInternal.Should().NotBeNull();
        getUserInternal.Should().BeEquivalentTo(_getUserExternalInternalResponse);

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";

        _queryDispatcherMock
            .Setup(x => x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == email), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"User with email {email} not found."));

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() =>
            TestCandidate.GetUserByEmailAsync(email, It.IsAny<CancellationToken>()));

        string expectedExceptionMessage = $"User with email {email} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}