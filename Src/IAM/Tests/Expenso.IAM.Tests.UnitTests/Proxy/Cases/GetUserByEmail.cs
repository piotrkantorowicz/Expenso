using Expenso.IAM.Core.DTO;
using Expenso.IAM.Proxy.Contracts;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByEmail : IamApiTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();
        UserServiceMock.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync(_userDto);

        // Act
        UserContract? user = await TestCandidate.GetUserByEmail(email);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userContract);
        UserServiceMock.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();
        UserServiceMock.Setup(x => x.GetUserByEmail(It.IsAny<string>())).ReturnsAsync((UserDto?)null);

        // Act
        UserContract? user = await TestCandidate.GetUserByEmail(email);

        // Assert
        user.Should().BeNull();
        UserServiceMock.Verify(x => x.GetUserByEmail(It.IsAny<string>()), Times.Once);
    }
}