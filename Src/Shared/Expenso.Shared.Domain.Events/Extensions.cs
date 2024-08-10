using System.Reflection;

using Expenso.Shared.Domain.Events.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Domain.Events;

public static class Extensions
{
    public static IServiceCollection AddDomainEvents(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IDomainEventBroker, DomainEventBroker>();

        services.Scan(action: selector =>
            selector
                .FromAssemblies(assemblies: assemblies)
                .AddClasses(action: c => c.AssignableTo(type: typeof(IDomainEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}