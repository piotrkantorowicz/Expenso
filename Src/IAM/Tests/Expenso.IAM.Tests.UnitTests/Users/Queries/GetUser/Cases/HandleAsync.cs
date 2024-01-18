using System.Text;

using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Queries.GetUser;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.Cases;

internal sealed class HandleAsync : GetUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserQuery query = new GetUserQuery(_userId);
        _userServiceMock.Setup(x => x.GetUserByIdAsync(_userId)).ReturnsAsync(_getUserResponse);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserResponse);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserQuery query = new GetUserQuery(Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(_userEmail)).ReturnsAsync(_getUserResponse);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new GetUserQuery(_userId);
        _userServiceMock.Setup(x => x.GetUserByIdAsync(_userId))!.ReturnsAsync((GetUserResponse?)null);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new GetUserQuery(Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(_userEmail))!.ReturnsAsync((GetUserResponse?)null);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query);

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserQuery query = new GetUserQuery();

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.HandleAsync(query));

        string expectedExceptionMessage = new StringBuilder()
            .Append(nameof(query.Id))
            .Append(" or ")
            .Append(nameof(query.Email))
            .Append(" must be provided.")
            .ToString();

        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}