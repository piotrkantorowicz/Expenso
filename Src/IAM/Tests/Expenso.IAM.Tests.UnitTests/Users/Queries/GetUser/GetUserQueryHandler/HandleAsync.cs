using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserQueryHandler;

internal sealed class HandleAsync : GetUserQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(MessageContext: _messageContextMock.Object, UserId: _userId);
        _userServiceMock.Setup(expression: x => x.GetUserByIdAsync(_userId)).ReturnsAsync(value: _getUserResponse);

        // Act
        GetUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserResponse);
    }

    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserQuery query = new(MessageContext: _messageContextMock.Object, Email: _userEmail);

        _userServiceMock
            .Setup(expression: x => x.GetUserByEmailAsync(_userEmail))
            .ReturnsAsync(value: _getUserResponse);

        // Act
        GetUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(MessageContext: _messageContextMock.Object, UserId: _userId);

        _userServiceMock.Setup(expression: x => x.GetUserByIdAsync(_userId))!.ReturnsAsync(
            value: (GetUserResponse?)null);

        // Act
        GetUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserQuery query = new(MessageContext: _messageContextMock.Object, Email: _userEmail);

        _userServiceMock.Setup(expression: x => x.GetUserByEmailAsync(_userEmail))!.ReturnsAsync(
            value: (GetUserResponse?)null);

        // Act
        GetUserResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserQuery query = new(MessageContext: _messageContextMock.Object);

        // Act
        // Assert
        NotFoundException? exception = Assert.ThrowsAsync<NotFoundException>(code: () =>
            TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>()));

        const string expectedExceptionMessage = $"{nameof(query.UserId)} or {nameof(query.Email)} must be provided.";
        exception?.Message.Should().Be(expected: expectedExceptionMessage);
    }
}