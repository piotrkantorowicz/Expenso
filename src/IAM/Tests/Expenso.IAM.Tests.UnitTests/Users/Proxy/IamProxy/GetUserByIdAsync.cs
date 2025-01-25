using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Shared.DTO.GetUserById.Request;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.Is<GetUserByIdQuery>(y => y.Payload!.UserId == _userId),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByIdResponse);

        // Act
        GetUserByIdResponse? getUserResponse = await TestCandidate.GetUserByIdAsync(
            request: new GetUserByIdRequest(UserId: _userId), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.ShouldNotBeNull();
        getUserResponse.ShouldBeEquivalentTo(expected: _getUserByIdResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(It.Is<GetUserByIdQuery>(y => y.Payload!.UserId == _userId),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowsNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.Is<GetUserByIdQuery>(y => y.Payload!.UserId == userId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(resourceName: "User",
                identifierType: IdentifierType.PrimaryId(), identifier: userId));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByIdAsync(request: new GetUserByIdRequest(UserId: userId),
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"User with ID {userId} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.PrimaryId());
        exception.Identifier.ShouldBe(expected: userId);
    }
}