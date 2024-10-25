using System.Reflection;

using Expenso.Shared.System.Configuration.Binders;
using Expenso.Shared.System.Configuration.Services;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.System.Configuration;

public static class Extensions
{
    public static IServiceCollection AddSettings(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IValidator<>)))
                .AsImplementedInterfaces()
                .WithSingletonLifetime());

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