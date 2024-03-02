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
            .Setup(x => x.GetService(typeof(IMessageContextFactory)))
            .Returns(MessageContextFactoryMock.Object);

        MessageContextFactoryResolver.BindResolver(serviceProviderMock.Object);
    }

    protected static void AssertDomainEventPublished(IAggregateRoot aggregateRoot,
        IEnumerable<IDomainEvent> expectedDomainEvents)
    {
        expectedDomainEvents
            .Should()
            .BeEquivalentTo(aggregateRoot.GetUncommittedChanges(),
                options => options.IncludingNestedObjects().WithStrictOrdering().RespectingRuntimeTypes());
    }
}