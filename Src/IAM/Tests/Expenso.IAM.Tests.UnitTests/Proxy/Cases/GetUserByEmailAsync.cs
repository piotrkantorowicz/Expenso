using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _userServiceMock.Setup(x => x.GetUserByEmailInternalAsync(_userEmail)).ReturnsAsync(_userContract);

        // Act
        UserContract user = await TestCandidate.GetUserByEmailAsync(_userEmail);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userContract);
        _userServiceMock.Verify(x => x.GetUserByEmailInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";

        _userServiceMock
            .Setup(x => x.GetUserByEmailInternalAsync(email))
            .ThrowsAsync(new NotFoundException($"User with email {email} not found."));

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByEmailAsync(email));

        string expectedExceptionMessage =
            new StringBuilder().Append("User with email ").Append(email).Append(" not found.").ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}