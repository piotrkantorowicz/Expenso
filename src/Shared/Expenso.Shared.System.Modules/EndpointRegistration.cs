namespace Expenso.Shared.System.Modules;

public sealed record EndpointRegistration(
    string Pattern,
    string Name,
    AccessControl AccessControl,
    HttpVerb HttpVerb,
    Delegate? Handler,
    string? SubModule = null);

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