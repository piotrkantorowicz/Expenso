using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        UserServiceMock.Setup(x => x.GetUserByIdInternalAsync(UserId)).ReturnsAsync(UserContract);

        // Act
        UserContract user = await TestCandidate.GetUserByIdAsync(UserId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserContract);
        UserServiceMock.Verify(x => x.GetUserByIdInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        UserServiceMock
            .Setup(x => x.GetUserByIdInternalAsync(userId))
            .ThrowsAsync(new NotFoundException($"User with id {userId} not found."));

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByIdAsync(userId));

        exception
            ?.Message.Should()
            .Be(new StringBuilder().Append("User with id ").Append(userId).Append(" not found.").ToString());
    }
}