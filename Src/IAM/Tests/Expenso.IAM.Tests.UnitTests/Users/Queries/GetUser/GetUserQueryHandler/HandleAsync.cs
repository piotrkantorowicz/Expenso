using Expenso.IAM.Core.Users.DTO.GetUser;
using Expenso.IAM.Core.Users.Queries.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserQueryHandler;

internal sealed class HandleAsync : GetUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, _userId);
        _userServiceMock.Setup(x => x.GetUserByIdAsync(_userId)).ReturnsAsync(_getUserResponse);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserResponse);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(_userEmail)).ReturnsAsync(_getUserResponse);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, _userId);
        _userServiceMock.Setup(x => x.GetUserByIdAsync(_userId))!.ReturnsAsync((GetUserResponse?)null);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailAsync(_userEmail))!.ReturnsAsync((GetUserResponse?)null);

        // Act
        GetUserResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object);

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() =>
                TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>()));

        const string expectedExceptionMessage = $"{nameof(query.UserId)} or {nameof(query.Email)} must be provided.";
        exception?.Message.Should().Be(expectedExceptionMessage);
    }
}