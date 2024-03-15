using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.IAM.Proxy;

public static class Extensions
{
    public static void AddIamProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IIamProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}