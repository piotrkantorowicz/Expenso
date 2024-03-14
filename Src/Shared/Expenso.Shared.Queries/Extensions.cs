using System.Reflection;

using Expenso.Shared.Queries.Dispatchers;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Queries;

public static class Extensions
{
    public static IServiceCollection AddQueries(this IServiceCollection services, IEnumerable<Assembly> assemblies)
    {
        services.AddSingleton<IQueryDispatcher, QueryDispatcher>();

        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IQueryHandler<,>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}