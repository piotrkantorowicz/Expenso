using Expenso.Shared.Types.Exceptions;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Expenso.Api.Tests.UnitTests.Configuration.Filters.Cases;

internal sealed class OnException : ApiExceptionFilterAttributeTestBase
{
    [Test]
    public void Should_Return401_When_AuthorizationExceptionThrown()
    {
        // Arrange
        _exceptionContext.Exception = new UnauthorizedException("Unauthorized");

        // Act
        TestCandidate.OnException(_exceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)_exceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public void Should_Return403_When_ForbiddenExceptionThrown()
    {
        // Arrange
        _exceptionContext.Exception = new ForbiddenException("Forbidden");

        // Act
        TestCandidate.OnException(_exceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)_exceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Test]
    public void Should_Return404_When_NotFoundExceptionThrown()
    {
        // Arrange
        _exceptionContext.Exception = new NotFoundException("Not found");

        // Act
        TestCandidate.OnException(_exceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)_exceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Test]
    public void Should_Return422_When_ValidationFailed()
    {
        // Arrange
        _actionContext.ModelState.AddModelError("Error", "Message");

        // Act
        TestCandidate.OnException(_exceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)_exceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
    }

    [Test]
    public void Should_Return500_When_UnhandledExceptionThrown()
    {
        // Arrange
        _exceptionContext.Exception = new Exception();

        // Act
        TestCandidate.OnException(_exceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)_exceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}