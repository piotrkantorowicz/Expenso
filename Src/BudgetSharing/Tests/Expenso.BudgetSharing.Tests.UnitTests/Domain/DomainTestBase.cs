using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentAssertions;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain;

internal abstract class DomainTestBase<TTestCandidate> : TestBase<TTestCandidate> where TTestCandidate : class
{
    [OneTimeSetUp]
    public override void OneTimeSetUp()
    {
        base.OneTimeSetUp();
        MessageContextFactoryResolverInitializer.Initialize(MessageContextFactoryMock.Object);
    }

    protected static void AssertDomainEventPublished(IAggregateRoot aggregateRoot,
        IEnumerable<IDomainEvent> expectedDomainEvents)
    {
        IReadOnlyCollection<IDomainEvent> existingDomainEvents = aggregateRoot.GetUncommittedChanges();
        bool matchingDomainEvents = expectedDomainEvents.SequenceEqual(existingDomainEvents);
        matchingDomainEvents.Should().BeTrue();
    }
}