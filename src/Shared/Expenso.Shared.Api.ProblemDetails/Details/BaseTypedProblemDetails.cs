namespace Expenso.Shared.Api.ProblemDetails.Details;

public abstract record BaseTypedProblemDetails<TDetail> : BaseProblemDetails where TDetail : class
{
    public TDetail? Detail { get; set; }
}