using Expenso.IAM.Core.Application.Users.Read.Queries.GetUserByEmail;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Request;
using Expenso.IAM.Shared.DTO.GetUserByEmail.Response;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Models;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.IAM.Tests.UnitTests.Users.Proxy.IamProxy;

[TestFixture]
internal sealed class GetUserByEmailAsync : IamProxyTestBase
{
    [Test]
    public async Task Should_ReturnUser_When_UserExists()
    {
        // Arrange
        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.Is<GetUserByEmailQuery>(y => y.Payload!.Email == _userEmail),
                It.IsAny<CancellationToken>()))
            .ReturnsAsync(value: _getUserByEmailResponse);

        // Act
        GetUserByEmailResponse? getUserResponse = await TestCandidate.GetUserByEmailAsync(
            request: new GetUserByEmailRequest(Email: _userEmail), cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        getUserResponse.ShouldNotBeNull();
        getUserResponse.ShouldBeEquivalentTo(expected: _getUserByEmailResponse);

        _queryDispatcherMock.Verify(
            expression: x => x.QueryAsync(It.Is<GetUserByEmailQuery>(y => y.Payload!.Email == _userEmail),
                It.IsAny<CancellationToken>()), times: Times.Once);
    }

    [Test]
    public async Task Should_ThrowsNotFoundException_When_UserDoesNotExists()
    {
        // Arrange
        const string email = "email1@email.com";
        const string errorMessage = $"User with email {email} hasn't been found.";

        _queryDispatcherMock
            .Setup(expression: x => x.QueryAsync(It.Is<GetUserByEmailQuery>(y => y.Payload!.Email == email),
                It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new NotFoundException(resourceName: "User", identifierType: IdentifierType.Email(),
                identifier: email));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.GetUserByEmailAsync(request: new GetUserByEmailRequest(Email: email),
                cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        NotFoundException? exception = await action.ShouldThrowAsync<NotFoundException>();
        exception.Message.ShouldBe(expected: $"User with email {email} hasn't been found.");
        exception.ResourceName.ShouldBe(expected: "User");
        exception.IdentifierType.ShouldBe(expected: IdentifierType.Email());
        exception.Identifier.ShouldBe(expected: email);
    }
}