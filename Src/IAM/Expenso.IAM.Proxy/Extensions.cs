using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Proxy;

public static class Extensions
{
    public static void AddIamProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IIamProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}