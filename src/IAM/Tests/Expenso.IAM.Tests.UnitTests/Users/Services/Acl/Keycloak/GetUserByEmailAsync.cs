using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

[TestFixture]
internal sealed class GetUserByEmailAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:
            [
                _user
            ]);

        // Act
        GetUserByEmailResponse getUser = await TestCandidate.GetUserByEmailAsync(
            request: new GetUserByEmailRequest(Email: _userEmail), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(expectation: _getUserByEmailResponse);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email@email.com";

        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: ArraySegment<UserRepresentation>.Empty);

        // Act
        Func<Task> action = async () => await TestCandidate.GetUserByEmailAsync(
            request: new GetUserByEmailRequest(Email: email), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with email {email} hasn't been found.")
            .Where(exceptionExpression: x => x.ResourceName == "User" && x.IdentifierType == IdentifierType.Email() &&
                                             (string?)x.Identifier == email);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_UserDoesNotHaveUniqueIdentyifier()
    {
        // Arrange
        const string email = "email@email.com";

        _keycloakUserClientMock
            .Setup(expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value:
            [
                new UserRepresentation
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "carolhussain@email.com"
                },
                new UserRepresentation
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "carolhussain@email.com"
                }
            ]);

        // Act
        Func<Task> action = async () => await TestCandidate.GetUserByEmailAsync(
            request: new GetUserByEmailRequest(Email: email), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        await action
            .Should()
            .ThrowAsync<ConflictException>()
            .WithMessage(expectedWildcardPattern: $"User with email {email} hasn't been uniquely identified.")
            .Where(exceptionExpression: x => x.ResourceName == "User" && x.IdentifierType == IdentifierType.Email() &&
                                             (string?)x.Identifier == email);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}