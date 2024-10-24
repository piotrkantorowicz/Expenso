﻿using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserById;
using Expenso.IAM.Shared.DTO.GetUserById.Response;
using Expenso.Shared.System.Types.Exceptions;

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
        GetUserByIdResponse? getUserResponse =
            await TestCandidate.GetUserByIdAsync(userId: _userId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.Should().NotBeNull();
        getUserResponse.Should().BeEquivalentTo(expectation: _getUserByIdResponse);

        _queryDispatcherMock.Verify(expression: x =>
            x.QueryAsync(It.Is<GetUserByIdQuery>(y => y.Payload!.UserId == _userId),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.Is<GetUserByIdQuery>(y => y.Payload!.UserId == _userId),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(message: $"User with ID {userId} not found."));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByIdAsync(userId: userId, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        action
            .Should()
            .ThrowAsync<NotFoundException>()
            .WithMessage(expectedWildcardPattern: $"User with ID {userId} not found");
    }
}