using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Logging;
using Expenso.Shared.Domain.Types.Aggregates;

namespace Expenso.Shared.Tests.ArchTests.Assemblies;

internal static class DomainAssemblies
{
    private static readonly Assembly DomainEvents = typeof(IDomainEventHandler<>).Assembly;
    private static readonly Assembly DomainEventsLogging = typeof(DomainEventHandlerLoggingDecorator<>).Assembly;
    private static readonly Assembly DomainEventsTypes = typeof(IAggregateRoot).Assembly;

    private static readonly Dictionary<string, Assembly> Assemblies = new()
    {
        [key: nameof(DomainEvents)] = DomainEvents,
        [key: nameof(DomainEventsLogging)] = DomainEventsLogging,
        [key: nameof(DomainEventsTypes)] = DomainEventsTypes
    };

    public static IReadOnlyDictionary<string, Assembly> GetAssemblies()
    {
        return Assemblies;
    }
}