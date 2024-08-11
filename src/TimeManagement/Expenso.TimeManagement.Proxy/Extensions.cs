using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.TimeManagement.Proxy;

public static class Extensions
{
    public static void AddTimeManagementProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ITimeManagementProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}