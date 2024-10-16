using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.BudgetSharing.Shared;

public static class Extensions
{
    public static void AddBudgetSharingProxy(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IBudgetSharingProxy)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());
    }
}