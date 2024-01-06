using Expenso.Shared.Types.Exceptions;

using Microsoft.AspNetCore.Http;

namespace Expenso.Api.Tests.UnitTests.Configuration.Filters.Cases;

internal sealed class OnException : ApiExceptionFilterAttributeTestBase
{
    [Test]
    public async Task Should_Return401_When_AuthorizationExceptionThrown()
    {
        // Arrange
        UnauthorizedException exception = new UnauthorizedException("Unauthorized");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public async Task Should_Return403_When_ForbiddenExceptionThrown()
    {
        // Arrange
        ForbiddenException exception = new ForbiddenException("Forbidden");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Test]
    public async Task Should_Return404_When_NotFoundExceptionThrown()
    {
        // Arrange
        NotFoundException exception = new NotFoundException("Not found");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Test]
    public async Task Should_Return422_When_ValidationFailed()
    {
        // Arrange
        ValidationException exception = new ValidationException("Validation failed");

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
    }

    [Test]
    public async Task Should_Return500_When_UnhandledExceptionThrown()
    {
        // Arrange
        Exception exception = new Exception();

        // Act
        await TestCandidate.TryHandleAsync(_httpContext, exception, default);

        // Assert
        _httpContext.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}