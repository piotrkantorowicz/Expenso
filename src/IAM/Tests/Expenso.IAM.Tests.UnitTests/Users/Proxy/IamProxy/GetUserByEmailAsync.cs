using Expenso.IAM.Core.Application.Users.Read.Queries.GetUser;
using Expenso.IAM.Shared.DTO.GetUser;
using Expenso.Shared.System.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserResponse);

        // Act
        GetUserResponse? getUserResponse =
            await TestCandidate.GetUserByEmailAsync(email: _userEmail,
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.Should().NotBeNull();
        getUserResponse.Should().BeEquivalentTo(expectation: _getUserResponse);

        _queryDispatcherMock.Verify(
            expression: x =>
                x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == _userEmail), It.IsAny<CancellationToken>()),
            times: Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";

        _queryDispatcherMock
            .Setup(expression: x =>
                x.QueryAsync(It.Is<GetUserQuery>(y => y.Email == email), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(message: $"User with email {email} not found."));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByEmailAsync(email: email, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with email {email} not found");
    }
}