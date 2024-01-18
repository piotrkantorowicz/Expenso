﻿using System.Text;

using Expenso.IAM.Core.Users.Queries.GetUserInternal;
using Expenso.IAM.Proxy.DTO.GetUser;
using Expenso.Shared.Types.Exceptions;

namespace Expenso.IAM.Tests.UnitTests.Proxy.Cases;

internal sealed class GetUserByIdAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(x => x.QueryAsync(It.Is<GetUserInternalQuery>(y => y.Id == _userId), It.IsAny<CancellationToken>()))
            .ReturnsAsync(_getUserInternalResponse);

        // Act
        GetUserInternalResponse? getUserInternal = await TestCandidate.GetUserByIdAsync(_userId);

        // Assert
        getUserInternal.Should().NotBeNull();
        getUserInternal.Should().BeEquivalentTo(_getUserInternalResponse);

        _queryDispatcherMock.Verify(
            x => x.QueryAsync(It.Is<GetUserInternalQuery>(y => y.Id == _userId), It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Test]
    public void Should_ReturnNull_When_UserDoesNotExists()
    {
        // Arrange
        string userId = Guid.NewGuid().ToString();

        _queryDispatcherMock
            .Setup(x => x.QueryAsync(It.Is<GetUserInternalQuery>(y => y.Id == userId), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new NotFoundException($"User with id {userId} not found."));

        // Act
        // Assert
        NotFoundException? exception =
            Assert.ThrowsAsync<NotFoundException>(() => TestCandidate.GetUserByIdAsync(userId));

        string expectedExceptionMessage =
            new StringBuilder().Append("User with id ").Append(userId).Append(" not found.").ToString();
        
        exception
            ?.Message.Should()
            .Be(expectedExceptionMessage);
    }
}