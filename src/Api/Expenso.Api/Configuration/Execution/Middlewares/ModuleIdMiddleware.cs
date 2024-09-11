using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Modules;

namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class ModuleIdMiddleware
{
    internal const string ModuleMiddlewareHeaderKey = "Module";
    private readonly IReadOnlyCollection<string> _managementPaths = ["health", "metrics", "swagger"];
    private readonly RequestDelegate _next;

    public ModuleIdMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        ILoggerService<ModuleIdMiddleware>? logger =
            context.RequestServices.GetService<ILoggerService<ModuleIdMiddleware>>();

        string? moduleId = GuessModule(logger: logger, requestPath: context.Request.Path);
        context.Request.Headers.Append(key: ModuleMiddlewareHeaderKey, value: moduleId);

        context.Response.OnStarting(callback: () =>
        {
            context.Response.Headers[key: ModuleMiddlewareHeaderKey] = moduleId;

            return Task.CompletedTask;
        });

        await _next.Invoke(context: context);
    }

    private string? GuessModule(ILoggerService<ModuleIdMiddleware>? logger, string? requestPath)
    {
        if (requestPath is not null && _managementPaths.Any(predicate: requestPath.Contains))
        {
            logger?.LogDebug(eventId: LoggingUtils.GeneralInformation,
                message: "Management urls cannot be used to define module names");

            return null;
        }

        IDictionary<string, ModuleDefinition> registeredModules = Modules.GetRegisteredModules();

        if (!registeredModules.Any())
        {
            logger?.LogWarning(eventId: LoggingUtils.GeneralWarning, message: "No registered modules found");

            return null;
        }

        var modulePrefixes = Modules
            .GetRegisteredModules()
            .Select(selector: x => new
            {
                Prefix = x.Value.ModulePrefix,
                Name = x.Value.ModuleName
            })
            .ToList();

        foreach (var modulePrefix in modulePrefixes.Where(predicate: modulePrefix =>
                     requestPath?.Contains(value: modulePrefix.Prefix) == true))
        {
            logger?.LogDebug(eventId: LoggingUtils.GeneralInformation,
                message: $"Module found: {modulePrefix.Name} for request path: {requestPath}");

            return modulePrefix.Name;
        }

        logger?.LogWarning(eventId: LoggingUtils.GeneralWarning,
            message: $"No matching module found for request path: {requestPath}");

        return null;
    }
}