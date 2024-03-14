using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.DocumentManagement.Proxy;

public static class Extensions
{
    public static void AddDocumentManagementProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IDocumentManagementProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}