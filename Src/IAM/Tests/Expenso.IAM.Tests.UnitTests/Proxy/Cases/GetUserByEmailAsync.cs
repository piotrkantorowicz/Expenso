using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        UserServiceMock.Setup(x => x.GetUserByEmailInternalAsync(UserEmail)).ReturnsAsync(UserContract);

        // Act
        UserContract user = await TestCandidate.GetUserByEmailAsync(UserEmail);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(UserContract);
        UserServiceMock.Verify(x => x.GetUserByEmailInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";

        UserServiceMock
            .Setup(x => x.GetUserByEmailInternalAsync(email))
            .ThrowsAsync(new NotFoundException($"User with email {email} not found."));

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByEmailAsync(email));

        exception
            ?.Message.Should()
            .Be(new StringBuilder().Append("User with email ").Append(email).Append(" not found.").ToString());
    }
}