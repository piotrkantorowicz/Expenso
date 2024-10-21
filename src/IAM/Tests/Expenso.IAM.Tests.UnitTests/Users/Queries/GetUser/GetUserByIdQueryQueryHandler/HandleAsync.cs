using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByIdQueryQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUserByIdQueryQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByIdAndUserExists()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByIdRequest(UserId: _userId));

        _userServiceMock
            .Setup(expression: x => x.GetUserByIdAsync(_userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByIdResponse);

        // Act
        GetUserByIdResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserByIdResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByIdAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByIdRequest(UserId: _userId));

        _userServiceMock.Setup(expression: x => x.GetUserByIdAsync(_userId, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        GetUserByIdResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserByIdQuery query = new(MessageContext: _messageContextMock.Object, Payload: null);

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User not found. Payload: {query.Payload?.GetType().Name}");
    }
}