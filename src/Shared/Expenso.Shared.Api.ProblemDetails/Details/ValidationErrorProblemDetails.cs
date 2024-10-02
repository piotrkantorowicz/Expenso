using Expenso.Shared.System.Types.Exceptions.Validation;

namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record ValidationErrorProblemDetails : BaseTypedProblemDetails<IReadOnlyCollection<ValidationDetailModel>>
{
    private const string DefaultTitle = "Validation error occurred";

    public ValidationErrorProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
    }
}