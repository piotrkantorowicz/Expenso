using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Proxy;

public static class Extensions
{
    public static void AddUserPreferencesProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IUserPreferencesProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}