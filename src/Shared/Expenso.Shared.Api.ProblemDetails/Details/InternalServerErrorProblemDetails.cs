namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record InternalServerErrorProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "Internal server error";

    public InternalServerErrorProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.1";
        Detail = "An unexpected error occurred on the server.";
    }
}