using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

internal sealed class GetUserByIdAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), _userId)).ReturnsAsync(_user);

        // Act
        GetUserResponse getUser = await TestCandidate.GetUserByIdAsync(_userId);

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(_getUserResponse);
        _keycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), _userId), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        _keycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), userId))!.ReturnsAsync((User?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByIdAsync(userId));

        string expectedExceptionMessage = $"User with id {userId} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);
        _keycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }
}