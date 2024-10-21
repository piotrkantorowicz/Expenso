using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Queries.GetUser.GetUserByEmailQueryQueryHandler;

[TestFixture]
internal sealed class HandleAsync : GetUserByEmailQueryQueryHandlerTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_SearchingByEmailAndUserExists()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByEmailRequest(Email: _userEmail));

        _userServiceMock
            .Setup(expression: x => x.GetUserByEmailAsync(_userEmail, It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        // Act
        GetUserByEmailResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectation: _getUserByEmailResponse);
    }

    [Test]
    public async Task Should_ReturnNull_When_SearchingByEmailAndUserHasNotBeenFound()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object,
            Payload: new GetUserByEmailRequest(Email: _userEmail));

        _userServiceMock.Setup(expression: x => x.GetUserByEmailAsync(_userEmail, It.IsAny<CancellationToken>()))!
            .ReturnsAsync(value: null);

        // Act
        GetUserByEmailResponse? result =
            await TestCandidate.HandleAsync(query: query, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        result.Should().BeNull();
    }

    [Test]
    public void Should_ThrowNotFoundException_When_QueryIsEmpty()
    {
        // Arrange
        GetUserByEmailQuery query = new(MessageContext: _messageContextMock.Object, Payload: null);

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