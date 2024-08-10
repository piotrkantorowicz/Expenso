using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.TimeManagement.Proxy;

public static class Extensions
{
    public static void AddTimeManagementProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ITimeManagementProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}