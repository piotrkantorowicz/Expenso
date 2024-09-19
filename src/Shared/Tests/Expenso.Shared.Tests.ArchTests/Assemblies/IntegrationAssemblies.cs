using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.Events.Logging;
using Expenso.Shared.Integration.MessageBroker;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class IntegrationAssemblies
{
    private static readonly Assembly IntegrationEvents = typeof(IIntegrationEventHandler<>).Assembly;

    private static readonly Assembly IntegrationEventsLogging =
        typeof(IntegrationEventHandlerLoggingDecorator<>).Assembly;

    private static readonly Assembly IntegrationMessagebroker = typeof(IMessageBroker).Assembly;

    public static IReadOnlyCollection<Assembly> ToArray()
    {
        List<Assembly> assemblies =
        [
            IntegrationEvents,
            IntegrationEventsLogging,
            IntegrationMessagebroker
        ];

        return assemblies;
    }
}