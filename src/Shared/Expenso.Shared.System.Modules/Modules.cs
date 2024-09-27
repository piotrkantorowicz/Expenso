using System.Reflection;
using System.Text;

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

    public static void RegisterModules(Func<IModuleDefinition>[] moduleFactories)
    {
        foreach (Func<IModuleDefinition> moduleFactory in moduleFactories)
        {
            IModuleDefinition moduleDefinition = moduleFactory.Invoke();
            RegisteredModules.Add(key: moduleDefinition.ModuleName, value: moduleDefinition);
        }
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

                StringBuilder stringBuilder = new StringBuilder()
                    .Append(value: rootTag)
                    .Append(value: '.')
                    .Append(value: module.ModuleName);

                if (endpoint.SubModule is not null)
                {
                    stringBuilder.Append(value: '.');
                    stringBuilder.Append(value: endpoint.SubModule);
                }

                string tag = stringBuilder.ToString();
                routeHandlerBuilder.WithOpenApi().WithTags(tag);
            }
        }
    }

    public static void Clear()
    {
        RegisteredModules.Clear();
    }
}