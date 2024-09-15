using System.Reflection;

using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Services;
using Expenso.Shared.System.Configuration.Validators;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration;

public static class Extensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ISettingsValidator<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ISettingsBinder)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ISettingsService<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        return services;
    }
}