using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        UserServiceMock.Setup(x => x.GetUserByEmailInternalAsync(UserEmail)).ReturnsAsync(UserContract);

        // Act
        UserContract? user = await TestCandidate.GetUserByEmailAsync(UserEmail);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserContract);
        UserServiceMock.Verify(x => x.GetUserByEmailInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";
        UserServiceMock.Setup(x => x.GetUserByEmailInternalAsync(email)).ReturnsAsync((UserContract?)null);

        // Act
        UserContract? user = await TestCandidate.GetUserByEmailAsync(email);

        // Assert
        user.Should().BeNull();
        UserServiceMock.Verify(x => x.GetUserByEmailInternalAsync(email), Times.Once);
    }
}