using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Services.Acl.Keycloak;

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
        GetUserResponse getUser =
            await TestCandidate.GetUserByIdAsync(userId: _userId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUser.Should().NotBeNull();
        getUser.Should().BeEquivalentTo(expectation: _getUserResponse);

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUserAsync(It.IsAny<string>(), _userId, false, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _keycloakUserClientMock.Setup(expression: x =>
                x.GetUserAsync(It.IsAny<string>(), userId, false, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByIdAsync(userId: userId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with id {userId} not found.");

        _keycloakUserClientMock.Verify(
            expression: x => x.GetUserAsync(It.IsAny<string>(), userId, false, It.IsAny<CancellationToken>()),
            times: Times.Once);
    }
}