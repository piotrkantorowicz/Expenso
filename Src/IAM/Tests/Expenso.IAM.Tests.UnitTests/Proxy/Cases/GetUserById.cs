using Expenso.IAM.Proxy.DTO;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserById : IamApiTestBase
{
    [Test]
    public async Task ShouldReturnUser_When_UserExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();

        UserServiceMock.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync(_userDto);

        // Act
        UserDto? user = await TestCandidate.GetUserById(email);

        // Assert
        user.Should().NotBeNull();
        user.Should().BeEquivalentTo(_userDto);

        UserServiceMock.Verify(x => x.GetUserById(It.IsAny<string>()), Times.Once);
    }

    [Test]
    public async Task ShouldReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string? email = AutoFixtureProxy.Create<string>();

        UserServiceMock.Setup(x => x.GetUserById(It.IsAny<string>())).ReturnsAsync((UserDto?)null);

        // Act
        UserDto? user = await TestCandidate.GetUserById(email);

        // Assert
        user.Should().BeNull();

        UserServiceMock.Verify(x => x.GetUserById(It.IsAny<string>()), Times.Once);
    }
}