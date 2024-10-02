namespace Expenso.Shared.Api.ProblemDetails.Details;

public sealed record NotFoundProblemDetails : BaseTypedProblemDetails<string>
{
    private const string DefaultTitle = "The specified resource was not found";

    public NotFoundProblemDetails()
    {
        Title = DefaultTitle;
        Type = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.5";
    }
}