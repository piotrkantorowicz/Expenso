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
        { typeof(UnauthorizedException), HandleUnauthorizedAccessException },
        { typeof(ForbiddenException), HandleForbiddenAccessException }
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

        StaticProblemDetailsSelector.RegisterCustom(statusCode,
            type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21", title: "Validation error",
            modelState: context.ModelState);

        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);

        context.Result = new UnprocessableEntityObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleNotFoundException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status404NotFound;

        NotFoundException? exception = context.Exception as NotFoundException;

        StaticProblemDetailsSelector.RegisterCustom(statusCode,
            type: "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5",
            title: "The specified resource was not found.", detail: exception?.Message);

        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);

        context.Result = new NotFoundObjectResult(details);
        context.ExceptionHandled = true;
    }

    private static void HandleUnauthorizedAccessException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status401Unauthorized;

        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);
        context.Result = new ObjectResult(details) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }

    private static void HandleForbiddenAccessException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status403Forbidden;

        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);
        context.Result = new ObjectResult(details) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }

    private static void HandleUnknownException(ExceptionContext context)
    {
        const int statusCode = StatusCodes.Status500InternalServerError;

        ProblemDetails details = StaticProblemDetailsSelector.Select(statusCode);
        context.Result = new ObjectResult(details) { StatusCode = statusCode };
        context.ExceptionHandled = true;
    }
}