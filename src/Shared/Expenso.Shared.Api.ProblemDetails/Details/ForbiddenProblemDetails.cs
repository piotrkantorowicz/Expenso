namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record ForbiddenProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "Access forbidden";

    public ForbiddenProblemDetails()
    {
        Title = "Access forbidden";
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.4";
    }
}