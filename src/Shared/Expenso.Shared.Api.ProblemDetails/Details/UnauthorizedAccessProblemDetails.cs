namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record UnauthorizedAccessProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "Unauthorized access";

    public UnauthorizedAccessProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.3";
    }
}