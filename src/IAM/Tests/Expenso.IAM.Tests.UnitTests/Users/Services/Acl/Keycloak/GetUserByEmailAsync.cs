using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Keycloak.AuthServices.Sdk.Admin.Models;
using Keycloak.AuthServices.Sdk.Admin.Requests.Users;

using Moq;

using NUnit.Framework;

using Shouldly;

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
        getUser.ShouldNotBeNull();
        getUser.ShouldBeEquivalentTo(expected: _getUserByEmailResponse);

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
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"User with email {email} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Email());
        exception.Identifier.ShouldBe(expected: email);

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
        ConflictException? exception = await action.ShouldThrowAsync<ConflictException>();
        exception.Message.ShouldBe(expected: $"User with email {email} hasn't been uniquely identified.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Email());
        exception.Identifier.ShouldBe(expected: email);
        
        _keycloakUserClientMock.Verify(
            expression: x => x.GetUsersAsync(It.IsAny<string>(),
                It.Is<GetUsersRequestParameters>(y => y.Email == email), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}