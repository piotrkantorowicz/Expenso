using Expenso.IAM.Core.DTO;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByEmailAsync : UserServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        KeycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == UserEmail)))
            .ReturnsAsync(new[]
            {
                User
            });

        // Act
        UserDto? user = await TestCandidate.GetUserByEmailAsync(UserEmail);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserDto);

        KeycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == UserEmail)),
            Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email@email.com";

        KeycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)))
            .ReturnsAsync(ArraySegment<User>.Empty);

        // Act
        UserDto? user = await TestCandidate.GetUserByEmailAsync(email);

        // Assert
        user.Should().BeNull();

        KeycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)), Times.Once);
    }
}