using System.Text;

using Expenso.IAM.Proxy.Contracts;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _userServiceMock.Setup(x => x.GetUserByIdInternalAsync(_userId)).ReturnsAsync(_userContract);

        // Act
        UserContract user = await TestCandidate.GetUserByIdAsync(_userId);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userContract);
        _userServiceMock.Verify(x => x.GetUserByIdInternalAsync(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _userServiceMock
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