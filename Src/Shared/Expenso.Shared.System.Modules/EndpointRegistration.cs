namespace Expenso.Shared.System.Modules;

public sealed record EndpointRegistration(
    string Pattern,
    string Name,
    AccessControl AccessControl,
    HttpVerb HttpVerb,
    Delegate? Handler);

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