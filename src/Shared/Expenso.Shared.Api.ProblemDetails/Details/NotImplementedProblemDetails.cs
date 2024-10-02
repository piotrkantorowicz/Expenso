namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record NotImplementedProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "Not implemented";

    public NotImplementedProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.1";
        Detail = "The requested operation is not implemented.";
    }
}