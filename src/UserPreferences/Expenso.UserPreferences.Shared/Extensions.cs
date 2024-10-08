using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.UserPreferences.Shared;

public static class Extensions
{
    public static void AddUserPreferencesProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IUserPreferencesProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}