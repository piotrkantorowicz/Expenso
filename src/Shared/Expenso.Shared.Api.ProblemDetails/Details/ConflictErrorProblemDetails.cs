namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record ConflictErrorProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "Conflict occurred";

    public ConflictErrorProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.2";
    }
}