using Expenso.Api.Configuration.ErrorDetails;
using Expenso.Shared.Types.Exceptions;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Expenso.Api.Configuration.Filters;

internal sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
{
    private readonly Dictionary<Type, Action<ExceptionContext>> _exceptionHandlers = new()
    {
        { typeof(NotFoundException), HandleNotFoundException },
        { typeof(ConflictException), HandleConflictException },
        { typeof(UnauthorizedException), HandleUnauthorizedAccessException },
        { typeof(ForbiddenException), HandleForbiddenAccessException },
        { typeof(ValidationException), HandleInvalidModelStateException }
    };

    public override void OnException(ExceptionContext context)
    {
        HandleException(context);
        base.OnException(context);
    }

    private void HandleException(ExceptionContext? context)
    {
        Exception? exception = context?.Exception;
        Type? type = exception?.GetType();

        if (type != null && _exceptionHandlers.TryGetValue(type, out Action<ExceptionContext>? handler))
        {
            handler.Invoke(context!);

            return;
        }

        if (context!.ModelState.IsValid == false)
        {
            HandleInvalidModelStateException(context);

            return;
        }

        HandleUnknownException(context);
    }

    private static void HandleInvalidModelStateException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status422UnprocessableEntity;
        ValidationException? exception = context.Exception as ValidationException;

        ProblemDetails details =
            StaticProblemDetailsSelector.Select(statusCode, exception?.Details, context.ModelState);

        context.Result = new UnprocessableEntityObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status404NotFound;
        NotFoundException? exception = context.Exception as NotFoundException;
        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode, exception?.Message);
        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleConflictException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status409Conflict;
        ConflictException? exception = context.Exception as ConflictException;
        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode, exception?.Message);
        context.Result = new ConflictObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status401Unauthorized;
        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);

        context.Result = new ObjectResult(details)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }

    private static void HandleForbiddenAccessException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status403Forbidden;
        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);

        context.Result = new ObjectResult(details)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;
        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);

        context.Result = new ObjectResult(details)
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}