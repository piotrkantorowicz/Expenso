using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByEmailInternalAsync : UserServiceTestBase
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
        UserContract user = await TestCandidate.GetUserByEmailInternalAsync(UserEmail);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserDto);

        KeycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == UserEmail)),
            Times.Once);
    }

    [Test]
    public void ShouldThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email@email.com";

        KeycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)))
            .ReturnsAsync(ArraySegment<User>.Empty);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByEmailAsync(email));

        exception
            ?.Message.Should()
            .Be(new StringBuilder().Append("User with email ").Append(email).Append(" not found.").ToString());

        KeycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)), Times.Once);
    }
}