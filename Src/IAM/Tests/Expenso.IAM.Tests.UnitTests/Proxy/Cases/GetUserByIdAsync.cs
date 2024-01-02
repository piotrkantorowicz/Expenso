using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        UserServiceMock.Setup(x => x.GetUserByIdInternalAsync(UserId)).ReturnsAsync(UserContract);

        // Act
        UserContract? user = await TestCandidate.GetUserByIdAsync(UserId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserContract);
        UserServiceMock.Verify(x => x.GetUserByIdInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();
        UserServiceMock.Setup(x => x.GetUserByIdInternalAsync(userId)).ReturnsAsync((UserContract?)null);

        // Act
        UserContract? user = await TestCandidate.GetUserByIdAsync(userId);

        // Assert
        user.Should().BeNull();
        UserServiceMock.Verify(x => x.GetUserByIdInternalAsync(userId), Times.Once);
    }
}