using Expenso.Api.Configuration.Errors.Details;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions;

using Microsoft.AspNetCore.Diagnostics;

namespace Expenso.Api.Configuration.Errors;

internal sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly Dictionary<Type, Func<Exception, HttpContext, CancellationToken, Task>> _exceptionHandlers = new()
    {
        { typeof(NotFoundException), HandleNotFoundException },
        { typeof(ConflictException), HandleConflictException },
        { typeof(UnauthorizedException), HandleUnauthorizedAccessException },
        { typeof(ForbiddenException), HandleForbiddenAccessException },
        { typeof(ValidationException), HandleInvalidModelStateException },
        { typeof(DomainRuleValidationException), HandleInvalidModelStateException }
    };

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        logger.LogError(exception, "Exception occurred: {Message}", exception.Message);
        Type type = exception.GetType();

        if (_exceptionHandlers.TryGetValue(type, out Func<Exception, HttpContext, CancellationToken, Task>? handler))
        {
            await handler.Invoke(exception, httpContext, cancellationToken);

            return true;
        }

        await HandleUnknownException(exception, httpContext, cancellationToken);

        return true;
    }

    private static Task HandleInvalidModelStateException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status422UnprocessableEntity, httpContext, exception, cancellationToken);
    }

    private static Task HandleNotFoundException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status404NotFound, httpContext, exception, cancellationToken);
    }

    private static Task HandleConflictException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status409Conflict, httpContext, exception, cancellationToken);
    }

    private static Task HandleUnauthorizedAccessException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status401Unauthorized, httpContext, null, cancellationToken);
    }

    private static Task HandleForbiddenAccessException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status403Forbidden, httpContext, null, cancellationToken);
    }

    private static Task HandleUnknownException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(StatusCodes.Status500InternalServerError, httpContext, null, cancellationToken);
    }

    private static Task HandleException(int statusCode, HttpContext httpContext, Exception? exception,
        CancellationToken cancellationToken)
    {
        string? expectedExceptionMessage = exception switch
        {
            DomainRuleValidationException domainRuleValidationException => domainRuleValidationException.Details,
            ValidationException validationException => validationException.Details,
            _ => exception?.Message
        };

        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsJsonAsync(
            StaticProblemDetailsSelector.Select(statusCode, expectedExceptionMessage), cancellationToken);
    }
}