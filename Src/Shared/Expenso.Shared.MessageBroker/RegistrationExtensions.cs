using Expenso.Shared.MessageBroker.InMemory;
using Expenso.Shared.MessageBroker.InMemory.Background;
using Expenso.Shared.MessageBroker.InMemory.Channels;

using Microsoft.Extensions.DependencyInjection;

namespace Expenso.Shared.MessageBroker;

public static class RegistrationExtensions
{
    public static void AddMessageBroker(this IServiceCollection services)
    {
        services.AddSingleton<IMessageBroker, InMemoryMessageBroker>();
        services.AddSingleton<IMessageChannel, MessageChannel>();
        services.AddSingleton(typeof(BackgroundMessageProcessor));
    }
}