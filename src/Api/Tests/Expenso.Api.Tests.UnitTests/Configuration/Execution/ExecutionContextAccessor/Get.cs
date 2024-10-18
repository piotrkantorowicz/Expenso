using System.Security.Claims;

using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Auth.Users;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Types.ExecutionContext.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

namespace Expenso.Api.Tests.UnitTests.Configuration.Execution.ExecutionContextAccessor;

internal sealed class Get : ExecutionContextAccessorTestBase
{
    [Test]
    public void Should_ReturnNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        DefaultHttpContext httpContext = new()
        {
            User = new ClaimsPrincipal(identities:
            [
                new ClaimsIdentity()
            ])
        };

        _httpContextAccessorMock.SetupGet(expression: x => x.HttpContext).Returns(value: httpContext);

        // Act
        IExecutionContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserContext.Should().BeNull();
        testResult?.CorrelationId.Should().BeNull();
    }

    [Test]
    public void Should_ReturnEmptyContext_WhenTokenHasNoClaims()
    {
        // Arrange
        DefaultHttpContext httpContext = new()
        {
            User = new ClaimsPrincipal(identities:
            [
                new ClaimsIdentity(authenticationType: "rX8hkFW")
            ])
        };

        _httpContextAccessorMock.SetupGet(expression: x => x.HttpContext).Returns(value: httpContext);

        // Act
        IExecutionContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserContext?.UserId.Should().BeNull();
        testResult?.UserContext?.Username.Should().BeNull();
        testResult?.CorrelationId.Should().BeNull();
    }

    [Test]
    public void Should_ReturnContext_WhenTokenHasClaims()
    {
        // Arrange
        const string? username = "Phasellusfeugiat";
        string userId = Guid.NewGuid().ToString();
        UserContext expectedUser = new(UserId: userId, Username: username);
        Guid expectedCorrelationId = Guid.NewGuid();

        DefaultHttpContext httpContext = new()
        {
            Request =
            {
                Headers =
                {
                    new KeyValuePair<string, StringValues>(key: CorrelationIdMiddleware.CorrelationHeaderKey,
                        value: expectedCorrelationId.ToString())
                }
            },
            User = new ClaimsPrincipal(identities:
            [
                new ClaimsIdentity(identity: new ClaimsIdentity(authenticationType: "rX8hkFW"), claims:
                [
                    new Claim(type: ClaimNames.UserIdClaimName, value: userId),
                    new Claim(type: ClaimNames.UsernameClaimName, value: username)
                ])
            ])
        };

        _httpContextAccessorMock.SetupGet(expression: x => x.HttpContext).Returns(value: httpContext);

        // Act
        IExecutionContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserContext?.Should().NotBeNull();
        testResult?.UserContext?.UserId.Should().Be(expected: expectedUser.UserId);
        testResult?.UserContext?.Username.Should().Be(expected: expectedUser.Username);
        testResult?.CorrelationId.Should().NotBeNull();
        testResult?.CorrelationId.Should().Be(expected: expectedCorrelationId);
    }
}