using System.Text;

using Expenso.IAM.Core.DTO;
using Expenso.Shared.Types.Exceptions;

using Keycloak.AuthServices.Sdk.Admin.Models;

namespace Expenso.IAM.Tests.UnitTests.Services.Cases;

internal sealed class GetUserByIdAsync : UserServiceTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _keycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), _userId)).ReturnsAsync(_user);

        // Act
        UserDto user = await TestCandidate.GetUserByIdAsync(_userId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userDto);
        _keycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), _userId), Times.Once);
    }

    [Test]
    public void Should_ThrowNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        _keycloakUserClientMock.Setup(x => x.GetUser(It.IsAny<string>(), userId))!.ReturnsAsync((User?)null);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByIdAsync(userId));

        string expectedExceptionMessage =
            new StringBuilder().Append("User with id ").Append(userId).Append(" not found.").ToString();
        
        exception
            ?.Message.Should()
            .Be(expectedExceptionMessage);

        _keycloakUserClientMock.Verify(x => x.GetUser(It.IsAny<string>(), userId), Times.Once);
    }
}