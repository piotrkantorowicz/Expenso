using System.Reflection;

using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration;

public static class Extensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IValidator<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo<ISettingsBinder>(), publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(ISettingsService<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

        return services;
    }
}