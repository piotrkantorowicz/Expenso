using Expenso.IAM.Core.DTO;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByIdAsync : UserServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        KeycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), UserId)).ReturnsAsync(User);

        // Act
        UserDto? user = await TestCandidate.GetUserByIdAsync(UserId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserDto);
        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), UserId), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        KeycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), userId))!.ReturnsAsync((User?)null);

        // Act
        UserDto? user = await TestCandidate.GetUserByIdAsync(userId);

        // Assert
        user.Should().BeNull();
        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }
}