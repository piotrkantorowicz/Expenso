using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Communication.Proxy;

public static class Extensions
{
    public static void AddCommunicationProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(ICommunicationProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}