using Expenso.Shared.Api.ProblemDetails.Details;

namespace Expenso.Shared.System.Modules;

public sealed record EndpointRegistration
{
    public EndpointRegistration(string pattern, string name, AccessControl accessControl, HttpVerb httpVerb,
        Delegate? handler, string description, string summary, IEnumerable<Produces> responses,
        IEnumerable<Produces>? problems = null, string? subModule = null)
    {
        Pattern = pattern;
        Name = name;
        AccessControl = accessControl;
        HttpVerb = httpVerb;
        Handler = handler;
        Description = description;
        Summary = summary;
        Responses = responses.ToList();
        Problems = problems?.ToList() ?? FillProblemsBasedOnHttpVerb();
        SubModule = subModule;
    }

    public string Pattern { get; init; }

    public string Name { get; init; }

    public AccessControl AccessControl { get; init; }

    public HttpVerb HttpVerb { get; init; }

    public Delegate? Handler { get; init; }

    public string Description { get; init; }

    public string Summary { get; init; }

    public IReadOnlyList<Produces>? Responses { get; init; }

    public IReadOnlyList<Produces>? Problems { get; init; }

    public string? SubModule { get; init; }

    private IReadOnlyList<Produces>? FillProblemsBasedOnHttpVerb()
    {
        return HttpVerb switch
        {
            HttpVerb.Get =>
            [
                new Produces(StatusCode: 401, ContentType: typeof(UnauthorizedAccessProblemDetails)),
                new Produces(StatusCode: 403, ContentType: typeof(ForbiddenProblemDetails)),
                new Produces(StatusCode: 404, ContentType: typeof(NotFoundProblemDetails))
            ],
            HttpVerb.Post =>
            [
                new Produces(StatusCode: 400, ContentType: typeof(ValidationErrorProblemDetails)),
                new Produces(StatusCode: 401, ContentType: typeof(UnauthorizedAccessProblemDetails)),
                new Produces(StatusCode: 403, ContentType: typeof(ForbiddenProblemDetails)),
                new Produces(StatusCode: 404, ContentType: typeof(NotFoundProblemDetails)),
                new Produces(StatusCode: 409, ContentType: typeof(ConflictErrorProblemDetails))
            ],
            HttpVerb.Patch =>
            [
                new Produces(StatusCode: 400, ContentType: typeof(ValidationErrorProblemDetails)),
                new Produces(StatusCode: 401, ContentType: typeof(UnauthorizedAccessProblemDetails)),
                new Produces(StatusCode: 403, ContentType: typeof(ForbiddenProblemDetails)),
                new Produces(StatusCode: 404, ContentType: typeof(NotFoundProblemDetails)),
                new Produces(StatusCode: 409, ContentType: typeof(ConflictErrorProblemDetails))
            ],
            HttpVerb.Put =>
            [
                new Produces(StatusCode: 400, ContentType: typeof(ValidationErrorProblemDetails)),
                new Produces(StatusCode: 401, ContentType: typeof(UnauthorizedAccessProblemDetails)),
                new Produces(StatusCode: 403, ContentType: typeof(ForbiddenProblemDetails)),
                new Produces(StatusCode: 404, ContentType: typeof(NotFoundProblemDetails)),
                new Produces(StatusCode: 409, ContentType: typeof(ConflictErrorProblemDetails))
            ],
            HttpVerb.Delete =>
            [
                new Produces(StatusCode: 401, ContentType: typeof(UnauthorizedAccessProblemDetails)),
                new Produces(StatusCode: 403, ContentType: typeof(ForbiddenProblemDetails)),
                new Produces(StatusCode: 404, ContentType: typeof(NotFoundProblemDetails)),
                new Produces(StatusCode: 409, ContentType: typeof(ConflictErrorProblemDetails))
            ],
            _ => null
        };
    }
}

public sealed record Produces(int StatusCode, Type? ContentType = null);

public enum HttpVerb
{
    Get,
    Post,
    Patch,
    Put,
    Delete
}

public enum AccessControl
{
    Anonymous,
    User,
    Unknown
}