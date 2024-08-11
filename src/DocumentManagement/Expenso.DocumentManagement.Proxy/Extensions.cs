using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.DocumentManagement.Proxy;

public static class Extensions
{
    public static void AddDocumentManagementProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IDocumentManagementProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}