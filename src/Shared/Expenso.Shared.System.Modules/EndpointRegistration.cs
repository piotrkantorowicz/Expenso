namespace Expenso.Shared.System.Modules;

public sealed record EndpointRegistration(
    string Pattern,
    string Name,
    AccessControl AccessControl,
    HttpVerb HttpVerb,
    Delegate? Handler);

public enum HttpVerb
{
    None = 0,
    Get = 1,
    Post = 2,
    Patch = 3,
    Put = 4,
    Delete = 5
}

public enum AccessControl
{
    None = 0,
    Anonymous = 1,
    User = 2
}