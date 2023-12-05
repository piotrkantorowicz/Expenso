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
        ExceptionContext.Exception = new UnauthorizedException("Unauthorized");

        // Act
        TestCandidate.OnException(ExceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)ExceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status401Unauthorized);
    }

    [Test]
    public void Should_Return403_When_ForbiddenExceptionThrown()
    {
        // Arrange
        ExceptionContext.Exception = new ForbiddenException("Forbidden");

        // Act
        TestCandidate.OnException(ExceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)ExceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status403Forbidden);
    }

    [Test]
    public void Should_Return404_When_NotFoundExceptionThrown()
    {
        // Arrange
        ExceptionContext.Exception = new NotFoundException("Not found");

        // Act
        TestCandidate.OnException(ExceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)ExceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status404NotFound);
    }

    [Test]
    public void Should_Return422_When_ValidationFailed()
    {
        // Arrange
        ActionContext.ModelState.AddModelError("Error", "Message");

        // Act
        TestCandidate.OnException(ExceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)ExceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status422UnprocessableEntity);
    }

    [Test]
    public void Should_Return500_When_UnhandledExceptionThrown()
    {
        // Arrange
        ExceptionContext.Exception = new Exception();

        // Act
        TestCandidate.OnException(ExceptionContext);

        // Assert
        int? statusCode = ((ObjectResult)ExceptionContext.Result!).StatusCode;
        statusCode.Should().Be(StatusCodes.Status500InternalServerError);
    }
}