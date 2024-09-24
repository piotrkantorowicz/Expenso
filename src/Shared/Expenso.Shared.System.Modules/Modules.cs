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
    private static readonly Dictionary<string, IModuleDefinition> RegisteredModules = new();

    public static void RegisterModule<TModule>(Func<TModule>? moduleFactory = default) where TModule : IModuleDefinition
    {
        TModule moduleDefinition = moduleFactory is not null ? moduleFactory() : Activator.CreateInstance<TModule>();
        RegisteredModules.Add(key: moduleDefinition.ModuleName, value: moduleDefinition);
    }

    public static void AddModules(this IServiceCollection services, IConfiguration configuration)
    {
        foreach (IModuleDefinition module in RegisteredModules.Values)
        {
            module.AddDependencies(services: services, configuration: configuration);
        }
    }

    public static IDictionary<string, IModuleDefinition> GetRegisteredModules()
    {
        return RegisteredModules;
    }

    public static IReadOnlyCollection<Assembly> GetRequiredModulesAssemblies(Assembly[]? merge = null)
    {
        List<Assembly> moduleAssemblies =
            RegisteredModules.Values.SelectMany(selector: module => module.GetModuleAssemblies()).ToList();

        if (merge is not null && merge.Length > 0)
        {
            moduleAssemblies.AddRange(collection: merge);
        }

        return moduleAssemblies;
    }

    public static void MapModulesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder, string rootTag)
    {
        foreach (IModuleDefinition module in RegisteredModules.Values)
        {
            foreach (EndpointRegistration endpoint in module.CreateEndpoints())
            {
                string endpointRoute = $"{module.GetModulePrefixSanitized()}{endpoint.WithLeadingSlash().Pattern}";

                RouteHandlerBuilder routeHandlerBuilder = endpointRouteBuilder.MapMethods(pattern: endpointRoute,
                    httpMethods: new[]
                    {
                        endpoint.HttpVerb.ToString().ToUpper()
                    }, handler: endpoint.Handler!);

                switch (endpoint.AccessControl)
                {
                    case AccessControl.User:
                        routeHandlerBuilder.RequireAuthorization();

                        break;
                    case AccessControl.Anonymous:
                    case AccessControl.Unknown:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(paramName: endpoint.AccessControl.GetType().Name,
                            actualValue: endpoint.AccessControl, message: "Unknown access control type");
                }

                routeHandlerBuilder.WithName(endpointName: endpoint.Name);
                string tag = $"{rootTag}.{module.ModuleName}";

                if (endpoint.SubModule is not null)
                {
                    tag += $".{endpoint.SubModule}";
                }

                routeHandlerBuilder.WithOpenApi().WithTags(tag);
            }
        }
    }

    public static void Clear()
    {
        RegisteredModules.Clear();
    }
}