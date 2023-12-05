using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Api.Configuration.ErrorDetails;

internal static class StaticProblemDetailsSelector
{
    private static readonly Dictionary<int, ProblemDetails> ProblemDetailsMap = new()
    {
        {
            StatusCodes.Status401Unauthorized, new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2"
            }
        },
        {
            StatusCodes.Status403Forbidden, new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4"
            }
        },
        {
            StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"
            }
        },
        {
            StatusCodes.Status501NotImplemented, new ProblemDetails
            {
                Status = StatusCodes.Status501NotImplemented,
                Title = "Not implemented",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2"
            }
        }
    };

    public static ProblemDetails Select(int statusCode)
    {
        return ProblemDetailsMap[statusCode];
    }

    public static void RegisterCustom(int statusCode, string? detail = null, string? title = null, string? type = null,
        ModelStateDictionary? modelState = null)
    {
        if (ProblemDetailsMap.TryGetValue(statusCode, out ProblemDetails? problemDetails))
        {
            return;
        }

        if (modelState is not null)
        {
            problemDetails = new ValidationProblemDetails(modelState)
            {
                Status = statusCode,
                Detail = detail,
                Title = title,
                Type = type
            };

            ProblemDetailsMap.Add(statusCode, problemDetails);

            return;
        }

        problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Detail = detail,
            Title = title,
            Type = type
        };

        ProblemDetailsMap.Add(statusCode, problemDetails);
    }
}