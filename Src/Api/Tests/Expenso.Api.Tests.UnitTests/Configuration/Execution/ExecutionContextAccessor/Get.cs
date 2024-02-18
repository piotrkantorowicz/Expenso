using System.Security.Claims;

using Expenso.Api.Configuration.Auth.Claims;
using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Types.ExecutionContext.Models;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

using Context = Expenso.Api.Configuration.Auth.Users.UserContext;

namespace Expenso.Api.Tests.UnitTests.Configuration.Execution.ExecutionContextAccessor;

internal sealed class Get : ExecutionContextAccessorTestBase
{
    [Test]
    public void Should_ReturnNull_WhenUserIsNotAuthenticated()
    {
        // Arrange
        DefaultHttpContext httpContext = new()
        {
            User = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity()
            })
        };

        _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

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
            User = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity("rX8hkFW")
            })
        };

        _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

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
        Context expectedUser = new(userId, username);
        Guid expectedCorrelationId = Guid.NewGuid();

        DefaultHttpContext httpContext = new()
        {
            Request =
            {
                Headers =
                {
                    new KeyValuePair<string, StringValues>(CorrelationIdMiddleware.CorrelationHeaderKey,
                        expectedCorrelationId.ToString())
                }
            },
            User = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new ClaimsIdentity("rX8hkFW"), new[]
                {
                    new Claim(ClaimNames.UserIdClaimName, userId),
                    new Claim(ClaimNames.UsernameClaimName, username)
                })
            })
        };

        _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

        // Act
        IExecutionContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserContext?.Should().NotBeNull();
        testResult?.UserContext?.UserId.Should().Be(expectedUser.UserId);
        testResult?.UserContext?.Username.Should().Be(expectedUser.Username);
        testResult?.CorrelationId.Should().NotBeNull();
        testResult?.CorrelationId.Should().Be(expectedCorrelationId);
    }
}