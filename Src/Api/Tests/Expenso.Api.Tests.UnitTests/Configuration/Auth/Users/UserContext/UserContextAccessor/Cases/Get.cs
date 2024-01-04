using System.Security.Claims;

using Expenso.Shared.UserContext;

using Microsoft.AspNetCore.Http;

using Context = Expenso.Api.Configuration.Auth.Users.UserContext;

namespace Expenso.Api.Tests.UnitTests.Configuration.Auth.Users.UserContext.UserContextAccessor.Cases;

internal sealed class Get : UserContextAccessorTestBase
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
        IUserContext? testResult = TestCandidate.Get();

        // Assert
        testResult.Should().BeNull();
    }

    [Test]
    public void Should_ReturnEmptyContext_WhenTokenHasNoClaims()
    {
        // Arrange
        Context.UserContext expectedUser = new(null, null);

        DefaultHttpContext httpContext = new()
        {
            User = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity("rX8hkFW")
            })
        };

        _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

        // Act
        IUserContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserId.Should().Be(expectedUser.UserId);
        testResult?.Username.Should().Be(expectedUser.Username);
    }

    [Test]
    public void Should_ReturnContext_WhenTokenHasClaims()
    {
        // Arrange
        const string? username = "Phasellusfeugiat";
        string userId = Guid.NewGuid().ToString();
        Context.UserContext expectedUser = new(userId, username);

        DefaultHttpContext httpContext = new()
        {
            User = new ClaimsPrincipal(new[]
            {
                new ClaimsIdentity(new Claim[]
                {
                    new("user_id", userId),
                    new("name", username)
                })
            })
        };

        _httpContextAccessorMock.SetupGet(x => x.HttpContext).Returns(httpContext);

        // Act
        IUserContext? testResult = TestCandidate.Get();

        // Assert
        testResult?.UserId.Should().Be(expectedUser.UserId);
        testResult?.Username.Should().Be(expectedUser.Username);
    }
}