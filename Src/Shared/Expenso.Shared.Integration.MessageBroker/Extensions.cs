using System.Reflection;

using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.MessageBroker.InMemory;
using Expenso.Shared.Integration.MessageBroker.InMemory.Background;
using Expenso.Shared.Integration.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.Integration.MessageBroker;

public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton(typeof(BackgroundMessageProcessor));

        return services;
    }

    public static IServiceCollection AddIntegrationEvents(this IServiceCollection services,
        IEnumerable<Assembly> assemblies)
    {
        services.Scan(selector =>
            selector
                .FromAssemblies(assemblies)
                .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}