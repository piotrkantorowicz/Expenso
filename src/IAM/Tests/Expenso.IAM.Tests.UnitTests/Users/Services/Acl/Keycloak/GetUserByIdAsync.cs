using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

internal sealed class GetUserByIdAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock
            .Setup(expression: x => x.GetUser(It.IsAny<string>(), _userId))
            .ReturnsAsync(value: _user);

        // Act
        GetUserResponse getUser = await TestCandidate.GetUserByIdAsync(userId: _userId);

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(expectation: _getUserResponse);
        _keycloakUserClientMock.Verify(expression: x => x.GetUser(It.IsAny<string>(), _userId), times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _keycloakUserClientMock.Setup(expression: x => x.GetUser(It.IsAny<string>(), userId))!
            .ReturnsAsync(value: null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(code: () => TestCandidate.GetUserByIdAsync(userId: userId));

        string expectedExceptionMessage = $"User with id {userId} not found.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
        _keycloakUserClientMock.Verify(expression: x => x.GetUser(It.IsAny<string>(), userId), times: Times.Once);
    }
}