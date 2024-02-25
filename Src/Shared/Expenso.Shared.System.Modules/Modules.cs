using System.Reflection;

using Expenso.Shared.System.Modules.Extensions;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Modules;

public static class Modules
{
    private static readonly Dictionary<string, ModuleDefinition> RegisteredModules = new();

    public static void RegisterModule<TModule>(Func<TModule>? moduleFactory = default) where TModule : ModuleDefinition
    {
        TModule moduleDefinition = moduleFactory is not null ? moduleFactory() : Activator.CreateInstance<TModule>();
        RegisteredModules.Add(moduleDefinition.ModuleName, moduleDefinition);
    }

    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (ModuleDefinition module in RegisteredModules.Values)
        {
            module.AddDependencies(services, configuration);
        }
    }

    public static IReadOnlyCollection<Assembly> GetRequiredModulesAssemblies()
    {
        return RegisteredModules.Values.SelectMany(module => module.GetModuleAssemblies()).ToArray();
    }

    public static void MapModulesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, string rootTag)
    {
        foreach (ModuleDefinition module in RegisteredModules.Values)
        {
            foreach (EndpointRegistration endpoint in module.CreateEndpoints())
            {
                string endpointRoute = $"{module.GetModulePrefixSanitized()}{endpoint.WithLeadingSlash().Pattern}";

                RouteHandlerBuilder routeHandlerBuilder = endpointRouteBuilder.MapMethods(endpointRoute, new[]
                {
                    endpoint.HttpVerb.ToString().ToUpper()
                }, endpoint.Handler!);

                switch (endpoint.AccessControl)
                {
                    case AccessControl.User:
                        routeHandlerBuilder.RequireAuthorization();

                        break;
                    case AccessControl.Anonymous:
                    case AccessControl.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(endpoint.AccessControl.GetType().Name,
                            endpoint.AccessControl, "Unknown access control type.");
                }

                routeHandlerBuilder.WithName(endpoint.Name);
                string tag = $"{rootTag}.{module.ModuleName}";
                routeHandlerBuilder.WithOpenApi().WithTags(tag);
            }
        }
    }
}