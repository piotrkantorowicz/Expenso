using Expenso.Api.Configuration.Execution.Middlewares;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Modules;

namespace Expenso.Api.Configuration.Extensions;

internal static class RequestPathExtensions
{
    private static readonly IReadOnlyCollection<string> ManagementPaths = ["health", "metrics", "swagger"];

    internal static string? GuessModule(this string? requestPath, ILoggerService<ModuleIdMiddleware>? logger)
    {
        if (requestPath is null)
        {
            logger?.LogWarning(eventId: LoggingUtils.GeneralInformation, message: "Request path is null or empty");

            return null;
        }

        if (ManagementPaths.Any(predicate: requestPath.Contains))
        {
            logger?.LogDebug(eventId: LoggingUtils.GeneralInformation,
                message: "Management urls cannot be used to define module names");

            return null;
        }

        IDictionary<string, IModuleDefinition> registeredModules = Modules.GetRegisteredModules();

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