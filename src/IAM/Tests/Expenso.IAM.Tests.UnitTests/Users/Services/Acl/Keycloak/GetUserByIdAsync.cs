using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

[TestFixture]
internal sealed class GetUserByIdAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock
            .Setup(expression: x => x.GetUserAsync(It.IsAny<string>(), _userId, false, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _user);

        // Act
        GetUserByIdResponse getUser = await TestCandidate.GetUserByIdAsync(
            request: new GetUserByIdRequest(UserId: _userId), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUser.ShouldNotBeNull();
        getUser.ShouldBeEquivalentTo(expected: _getUserByIdResponse);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUserAsync(It.IsAny<string>(), _userId, false, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.CreateVersion7().ToString();

        _keycloakUserClientMock.Setup(expression: x =>
                x.GetUserAsync(It.IsAny<string>(), userId, false, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByIdAsync(request: new GetUserByIdRequest(UserId: userId),
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"User with ID {userId} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        exception.Identifier.ShouldBe(expected: userId);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUserAsync(It.IsAny<string>(), userId, false, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}