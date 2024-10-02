using Expenso.Shared.Api.ProblemDetails;
using Expenso.Shared.Commands.Validation.Exceptions;
using Expenso.Shared.Domain.Types.Exceptions;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Exceptions;
using Expenso.Shared.System.Types.Exceptions.Validation;

using Microsoft.AspNetCore.Diagnostics;

namespace Expenso.Api.Configuration.Errors;

internal sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly Dictionary<Type, Func<Exception, HttpContext, CancellationToken, Task>> _exceptionHandlers = new()
    {
        { typeof(NotFoundException), HandleNotFoundException },
        { typeof(ConflictException), HandleConflictException },
        { typeof(UnauthorizedException), HandleUnauthorizedAccessException },
        { typeof(ForbiddenException), HandleForbiddenAccessException },
        { typeof(ValidationException), HandleInvalidModelStateException },
        { typeof(CommandValidationException), HandleInvalidModelStateException },
        { typeof(DomainRuleValidationException), HandleInvalidModelStateException }
    };

    private readonly ILoggerService<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILoggerService<GlobalExceptionHandler> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(paramName: nameof(logger));
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(eventId: LoggingUtils.UnexpectedError, exception: exception,
            message: "Exception occurred: {Message}", args: exception.Message);

        Type type = exception.GetType();

        if (_exceptionHandlers.TryGetValue(key: type,
                value: out Func<Exception, HttpContext, CancellationToken, Task>? handler))
        {
            await handler.Invoke(arg1: exception, arg2: httpContext, arg3: cancellationToken);

            return true;
        }

        await HandleUnknownException(httpContext: httpContext, cancellationToken: cancellationToken);

        return true;
    }

    private static Task HandleInvalidModelStateException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status400BadRequest, httpContext: httpContext,
            exception: exception, cancellationToken: cancellationToken);
    }

    private static Task HandleNotFoundException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status404NotFound, httpContext: httpContext,
            exception: exception, cancellationToken: cancellationToken);
    }

    private static Task HandleConflictException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status409Conflict, httpContext: httpContext,
            exception: exception, cancellationToken: cancellationToken);
    }

    private static Task HandleUnauthorizedAccessException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status401Unauthorized, httpContext: httpContext, exception: null,
            cancellationToken: cancellationToken);
    }

    private static Task HandleForbiddenAccessException(Exception exception, HttpContext httpContext,
        CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status403Forbidden, httpContext: httpContext, exception: null,
            cancellationToken: cancellationToken);
    }

    private static Task HandleUnknownException(HttpContext httpContext, CancellationToken cancellationToken)
    {
        return HandleException(statusCode: StatusCodes.Status500InternalServerError, httpContext: httpContext,
            exception: null, cancellationToken: cancellationToken);
    }

    private static Task HandleException(int statusCode, HttpContext httpContext, Exception? exception,
        CancellationToken cancellationToken)
    {
        object? expectedExceptionMessage = exception switch
        {
            DomainRuleValidationException domainRuleValidationException => domainRuleValidationException.Errors,
            CommandValidationException validationException => validationException.Errors,
            ValidationException validationException => validationException.Errors,
            _ => exception?.Message
        };

        httpContext.Response.StatusCode = statusCode;

        return httpContext.Response.WriteAsJsonAsync(
            value: StaticProblemDetailsSelector.Select(statusCode: statusCode, detail: expectedExceptionMessage),
            cancellationToken: cancellationToken);
    }
}