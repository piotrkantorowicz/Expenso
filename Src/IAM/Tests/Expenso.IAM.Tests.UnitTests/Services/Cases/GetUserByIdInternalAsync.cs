using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByIdInternalAsync : UserServiceTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        KeycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), UserId)).ReturnsAsync(User);

        // Act
        UserContract user = await TestCandidate.GetUserByIdInternalAsync(UserId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserDto);
        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), UserId), Times.Once);
    }

    [Test]
    public void ShouldThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        KeycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), userId))!.ReturnsAsync((User?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByIdAsync(userId));

        exception
            ?.Message.Should()
            .Be(new StringBuilder().Append("User with id ").Append(userId).Append(" not found.").ToString());

        KeycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }
}