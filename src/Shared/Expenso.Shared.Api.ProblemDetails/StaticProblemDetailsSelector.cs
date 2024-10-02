using Expenso.Shared.Api.ProblemDetails.Details;
using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.Api.ProblemDetails;

public static class StaticProblemDetailsSelector
{
    private static readonly Dictionary<ProblemStatusCodes, BaseProblemDetails> ProblemDetailsMap = new()
    {
        { ProblemStatusCodes.BadRequest, new ValidationErrorProblemDetails() },
        { ProblemStatusCodes.Unauthorized, new UnauthorizedAccessProblemDetails() },
        { ProblemStatusCodes.Forbidden, new ForbiddenProblemDetails() },
        { ProblemStatusCodes.NotFound, new NotFoundProblemDetails() },
        { ProblemStatusCodes.Conflict, new ConflictErrorProblemDetails() },
        { ProblemStatusCodes.InternalServerError, new InternalServerErrorProblemDetails() },
        { ProblemStatusCodes.NotImplemented, new NotImplementedProblemDetails() }
    };

    public static object Select(int statusCode, object? detail = null)
    {
        if (!ProblemDetailsMap.TryGetValue((ProblemStatusCodes)statusCode, out var problemDetails))
        {
            throw new ArgumentOutOfRangeException(nameof(statusCode),
                $"The status code {statusCode} is not supported.");
        }

        if (detail is null)
        {
            return problemDetails;
        }

        switch (problemDetails)
        {
            case BaseTypedProblemDetails<string> problem:
                {
                    if (detail is not string detailString)
                    {
                        throw new InvalidOperationException(
                            $"The problem details type {problemDetails.GetType().Name} is not supported.");
                    }

                    return problem with
                    {
                        Detail = detailString
                    };
                }
            case BaseTypedProblemDetails<IReadOnlyCollection<ValidationDetailModel>> validationProblem:
                {
                    if (detail is not IReadOnlyCollection<ValidationDetailModel> validationDetailModels)
                    {
                        throw new InvalidOperationException(
                            $"The problem details type {problemDetails.GetType().Name} is not supported.");
                    }

                    return validationProblem with
                    {
                        Detail = validationDetailModels
                    };
                }
            default:
                throw new InvalidOperationException(
                    $"The problem details type {problemDetails.GetType().Name} is not supported.");
        }
    }
}