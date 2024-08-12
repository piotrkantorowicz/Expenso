using Expenso.IAM.Proxy.DTO.GetUser;
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
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: new[]
            {
                _user
            });

        // Act
        GetUserResponse getUser =
            await TestCandidate.GetUserByEmailAsync(email: _userEmail, It.IsAny<CancellationToken>());

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(expectation: _getUserResponse);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email@email.com";

        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: ArraySegment<UserRepresentation>.Empty);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.GetUserByEmailAsync(email: email, It.IsAny<CancellationToken>()));

        const string expectedExceptionMessage = $"User with email {email} not found";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}