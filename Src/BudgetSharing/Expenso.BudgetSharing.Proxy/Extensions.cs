using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Proxy;

public static class Extensions
{
    public static void AddBudgetSharingProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IBudgetSharingProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}