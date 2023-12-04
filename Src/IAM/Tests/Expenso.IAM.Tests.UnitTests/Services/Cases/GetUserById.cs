using Expenso.IAM.Proxy.DTO;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserById : UserServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        string userId = Guid
            .NewGuid()
            .ToString();

        KeycloakUserClientMock
            .Setup(x => x.GetUser(It.IsAny<string>(), userId))
            .ReturnsAsync(_user);

        // Act
        UserDto? user = await TestCandidate.GetUserById(userId);

        // Assert
        user
            .Should()
            .NotBeNull();

        user
            .Should()
            .BeEquivalentTo(_userDto);

        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid
            .NewGuid()
            .ToString();

        KeycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), userId))!.ReturnsAsync((User?)null);

        // Act
        UserDto? user = await TestCandidate.GetUserById(userId);

        // Assert
        user
            .Should()
            .BeNull();

        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }
}