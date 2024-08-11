using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Expenso.Api.Configuration.Errors.Details;

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
            StatusCodes.Status404NotFound, new ProblemDetails
            {
                Status = StatusCodes.Status404NotFound,
                Title = "The specified resource was not found.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5"
            }
        },
        {
            StatusCodes.Status409Conflict, new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Conflict occurred.",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10"
            }
        },
        {
            StatusCodes.Status422UnprocessableEntity, new ProblemDetails
            {
                Status = StatusCodes.Status422UnprocessableEntity,
                Title = "Validation error",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.21"
            }
        },
        {
            StatusCodes.Status500InternalServerError, new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Unknown error",
                Detail = "An error occurred while processing your request",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1"
            }
        },
        {
            StatusCodes.Status501NotImplemented, new ProblemDetails
            {
                Status = StatusCodes.Status501NotImplemented,
                Title = "Not implemented",
                Detail = "The requested resource is not implemented",
                Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2"
            }
        }
    };

    public static ProblemDetails Select(int statusCode, string? detail = null, ModelStateDictionary? modelState = null)
    {
        return ToDetailedResponse(problemDetails: ProblemDetailsMap[key: statusCode], detail: detail,
            modelState: modelState);
    }

    private static ProblemDetails ToDetailedResponse(ProblemDetails problemDetails, string? detail,
        ModelStateDictionary? modelState)
    {
        if (modelState is not null)
        {
            return new ValidationProblemDetails(modelState: modelState)
            {
                Status = problemDetails.Status,
                Title = problemDetails.Title,
                Type = problemDetails.Type,
                Detail = detail
            };
        }

        if (detail is not null)
        {
            problemDetails.Detail = detail;
        }

        return problemDetails;
    }
}