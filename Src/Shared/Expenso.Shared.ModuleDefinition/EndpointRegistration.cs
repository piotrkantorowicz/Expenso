namespace Expenso.Shared.ModuleDefinition;

public sealed record EndpointRegistration(string Pattern, string Name, AccessControl AccessControl, HttpVerb HttpVerb,
    Delegate Handler);

public enum HttpVerb
{
    Get,
    Post,
    Put,
    Delete
}

public enum AccessControl
{
    Anonymous,
    User,
    Unknown
}