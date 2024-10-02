namespace Expenso.Shared.Api.ProblemDetails.Details;

public abstract record BaseProblemDetails
{
    public string? Title { get; set; }

    public string? Type { get; set; }
}