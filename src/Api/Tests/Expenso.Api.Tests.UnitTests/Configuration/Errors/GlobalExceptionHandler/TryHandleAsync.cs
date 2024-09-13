using Expenso.Shared.System.Types.Exceptions;

using Microsoft.AspNetCore.Http;

namespace Expenso.Api.Tests.UnitTests.Configuration.Errors.GlobalExceptionHandler;

internal sealed class TryHandleAsync : GlobalExceptionHandlerTestBase
{
    [Test]
    public async Task Should_Return401_When_AuthorizationExceptionThrown()
    {
        // Arrange
        UnauthorizedException exception = new(message: "Unauthorized");

        // Act
        await TestCandidate.TryHandleAsync(httpContext: _httpContext, exception: exception, cancellationToken: default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(expected: StatusCodes.Status401Unauthorized);
    }

    [Test]
    public async Task Should_Return403_When_ForbiddenExceptionThrown()
    {
        // Arrange
        ForbiddenException exception = new(message: "Forbidden");

        // Act
        await TestCandidate.TryHandleAsync(httpContext: _httpContext, exception: exception, cancellationToken: default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(expected: StatusCodes.Status403Forbidden);
    }

    [Test]
    public async Task Should_Return404_When_NotFoundExceptionThrown()
    {
        // Arrange
        NotFoundException exception = new(message: "Not found");

        // Act
        await TestCandidate.TryHandleAsync(httpContext: _httpContext, exception: exception, cancellationToken: default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(expected: StatusCodes.Status404NotFound);
    }

    [Test]
    public async Task Should_Return422_When_ValidationFailed()
    {
        // Arrange
        ValidationException exception = new(details: "Validation failed");

        // Act
        await TestCandidate.TryHandleAsync(httpContext: _httpContext, exception: exception, cancellationToken: default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(expected: StatusCodes.Status422UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return500_When_UnhandledExceptionThrown()
    {
        // Arrange
        Exception exception = new();

        // Act
        await TestCandidate.TryHandleAsync(httpContext: _httpContext, exception: exception, cancellationToken: default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(expected: StatusCodes.Status500InternalServerError);
    }
}