using Expenso.Shared.System.Types.Exceptions;

using Microsoft.AspNetCore.Http;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

internal sealed class OnException : GlobalExceptionHandlerTestBase
{
    [Test]
    public async Task Should_Return401_When_AuthorizationExceptionThrown()
    {
        // Arrange
        UnauthorizedException exception = new("Unauthorized");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public async Task Should_Return403_When_ForbiddenExceptionThrown()
    {
        // Arrange
        ForbiddenException exception = new("Forbidden");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Test]
    public async Task Should_Return404_When_NotFoundExceptionThrown()
    {
        // Arrange
        NotFoundException exception = new("Not found.");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Test]
    public async Task Should_Return422_When_ValidationFailed()
    {
        // Arrange
        ValidationException exception = new("Validation failed");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return500_When_UnhandledExceptionThrown()
    {
        // Arrange
        Exception exception = new();

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}