using Expenso.IAM.Core.Users.Queries.GetUser.DTO.Response;
using Expenso.Shared.System.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

internal sealed class GetUserByEmailAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail)))
            .ReturnsAsync(new[]
            {
                _user
            });

        // Act
        GetUserResponse getUser = await TestCandidate.GetUserByEmailAsync(_userEmail);

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(_getUserResponse);

        _keycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail)),
            Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email@email.com";

        _keycloakUserClientMock
            .Setup(x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)))
            .ReturnsAsync(ArraySegment<User>.Empty);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByEmailAsync(email));

        const string expectedExceptionMessage = $"User with email {email} not found.";
        exception?.Message.Should().Be(expectedExceptionMessage);

        _keycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)), Times.Once);
    }
}