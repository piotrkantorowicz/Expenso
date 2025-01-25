using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain;

[TestFixture]
internal abstract class DomainTestBase<TTestCandidate> : TestBase<TTestCandidate> where TTestCandidate : class
{
    [OneTimeSetUp]
    public override void OneTimeSetUp()
    {
        base.OneTimeSetUp();
        Mock<IServiceProvider> serviceProviderMock = new();

        serviceProviderMock
            .Setup(expression: x => x.GetService(typeof(IMessageContextFactory)))
            .Returns(value: MessageContextFactoryMock.Object);

        MessageContextFactoryResolver.BindResolver(serviceProvider: serviceProviderMock.Object);
    }

    protected static void AssertDomainEventPublished(IAggregateRoot aggregateRoot,
        IEnumerable<IDomainEvent> expectedDomainEvents)
    {
        IDomainEvent[] expectedDomainEventsList = [..expectedDomainEvents];

        foreach (IDomainEvent? @event in aggregateRoot.GetUncommittedChanges())
        {
            IDomainEvent? expectedEvent = expectedDomainEventsList
                .FirstOrDefault(predicate: x => x.GetType() == @event.GetType())
                .ShouldNotBeNull();

            expectedEvent.ShouldDeepEqual(expected: @event);
        }
    }
}