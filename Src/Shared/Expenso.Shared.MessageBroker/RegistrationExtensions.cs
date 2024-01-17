using Expenso.Shared.IntegrationEvents;
using Expenso.Shared.MessageBroker.InMemory;
using Expenso.Shared.MessageBroker.InMemory.Background;
using Expenso.Shared.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.MessageBroker;

public static class RegistrationExtensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton(typeof(BackgroundMessageProcessor));

        services.Scan(selector =>
            selector
                .FromAssemblies(AppDomain.CurrentDomain.GetAssemblies())
                .AddClasses(c => c.AssignableTo(typeof(IIntegrationEventHandler<>)))
                .AsImplementedInterfaces()
                .WithScopedLifetime());

        return services;
    }
}