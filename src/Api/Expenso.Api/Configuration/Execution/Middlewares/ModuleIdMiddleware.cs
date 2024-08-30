using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Modules;

namespace Expenso.Api.Configuration.Execution.Middlewares;

internal sealed class ModuleIdMiddleware
{
    internal const string ModuleMiddlewareHeaderKey = "Module";
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
        await _next.Invoke(context: context);
    }

    private static string? GuessModule(ILoggerService<ModuleIdMiddleware>? logger, string? requestPath)
    {
        IDictionary<string, ModuleDefinition> registeredModules = Modules.GetRegisteredModules();

        if (!registeredModules.Any())
        {
            logger?.LogWarning(eventId: LoggingUtils.GeneralWarning, message: "No registered modules found.");

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
            logger?.LogInfo(eventId: LoggingUtils.GeneralInformation,
                message: $"Module found: {modulePrefix.Name} for request path: {requestPath}");

            return modulePrefix.Name;
        }

        logger?.LogWarning(eventId: LoggingUtils.GeneralWarning,
            message: $"No matching module found for request path: {requestPath}");

        return null;
    }
}