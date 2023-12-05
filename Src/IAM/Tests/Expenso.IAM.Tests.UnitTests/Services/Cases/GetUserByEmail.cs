using Expenso.IAM.Proxy.DTO;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByEmail : UserServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();

        KeycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>()))
            .ReturnsAsync(new[]
            {
                _user
            });

        // Act
        UserDto? user = await TestCandidate.GetUserByEmail(email);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userDto);

        KeycloakUserClientMock.Verify(x => x.GetUsers(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>()),
            Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();

        KeycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>()))
            .ReturnsAsync(ArraySegment<User>.Empty);

        // Act
        UserDto? user = await TestCandidate.GetUserByEmail(email);

        // Assert
        user.Should().BeNull();

        KeycloakUserClientMock.Verify(x => x.GetUsers(It.IsAny<string>(), It.IsAny<GetUsersRequestParameters>()),
            Times.Once);
    }
}