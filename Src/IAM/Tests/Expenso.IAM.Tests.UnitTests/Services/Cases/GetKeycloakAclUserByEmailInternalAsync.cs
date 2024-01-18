using System.Text;

using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetKeycloakAclUserByEmailInternalAsync : KeycloakAclUserServiceTestBase
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
        GetUserInternalResponse getUserInternal = await TestCandidate.GetUserByEmailInternalAsync(_userEmail);

        // Assert
        getUserInternal.Should().NotBeNull();
        getUserInternal.Should().BeEquivalentTo(_getUserResponse);

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

        string expectedExceptionMessage =
            new StringBuilder().Append("User with email ").Append(email).Append(" not found.").ToString();
        
        exception
            ?.Message.Should()
            .Be(expectedExceptionMessage);

        _keycloakUserClientMock.Verify(
            x => x.GetUsers(It.IsAny<string>(), It.Is<GetUsersRequestParameters>(y => y.Email == email)), Times.Once);
    }
}