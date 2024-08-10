using Expenso.BudgetSharing.Domain.Shared;
using Expenso.Shared.Domain.Types.Aggregates;
using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;
using Expenso.Shared.Tests.Utils.UnitTests;

using FluentAssertions;

using Moq;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain;

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
        expectedDomainEvents
            .Should()
            .BeEquivalentTo(expectation: aggregateRoot.GetUncommittedChanges(),
                config: options => options.IncludingNestedObjects().WithStrictOrdering().RespectingRuntimeTypes());
    }
}