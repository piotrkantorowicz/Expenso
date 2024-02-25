using Expenso.IAM.Core.Users.Internal.Queries.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Internal.Queries.GetUser.GetUserInternalQueryHandler;

internal sealed class HandleAsync : GetUserInternalQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, _userId);
        _userServiceMock.Setup(x => x.GetUserByIdInternalAsync(_userId)).ReturnsAsync(_getUserInternalResponse);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserInternalResponse);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, Email: _userEmail);
        _userServiceMock.Setup(x => x.GetUserByEmailInternalAsync(_userEmail)).ReturnsAsync(_getUserInternalResponse);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(_getUserInternalResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, _userId);
        _userServiceMock.Setup(x => x.GetUserByIdInternalAsync(_userId))!.ReturnsAsync((GetUserInternalResponse?)null);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(_messageContextMock.Object, Email: _userEmail);

        _userServiceMock.Setup(x => x.GetUserByEmailInternalAsync(_userEmail))!.ReturnsAsync(
            (GetUserInternalResponse?)null);

        // Act
        GetUserInternalResponse? result = await TestCandidate.HandleAsync(query, It.IsAny<CancellationToken>());

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