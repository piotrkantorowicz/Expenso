using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Proxy;

public static class Extensions
{
    public static void AddCommunicationProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ICommunicationProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}