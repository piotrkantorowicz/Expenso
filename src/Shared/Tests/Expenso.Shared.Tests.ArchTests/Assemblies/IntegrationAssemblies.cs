using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.Events.Logging;
using Expenso.Shared.Integration.MessageBroker;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class IntegrationAssemblies
{
    private static readonly Assembly IntegrationEvents = typeof(IIntegrationEventHandler<>).Assembly;

    private static readonly Assembly IntegrationEventsLogging =
        typeof(IntegrationEventHandlerLoggingDecorator<>).Assembly;
    private static readonly Assembly IntegrationMessageBroker = typeof(IMessageBroker).Assembly;

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(IntegrationEvents)] = IntegrationEvents,
        [key: nameof(IntegrationEventsLogging)] = IntegrationEventsLogging,
        [key: nameof(IntegrationMessageBroker)] = IntegrationMessageBroker
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}
