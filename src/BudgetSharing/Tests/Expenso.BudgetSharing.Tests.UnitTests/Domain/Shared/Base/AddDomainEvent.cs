using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.Tests.Utils.UnitTests.Assertions;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base;

[TestFixture]
internal sealed class AddDomainEvent : DomainEventsSourceTestBase
{
    [Test]
    public void AddDomainEvent_WhenCalled_AddsDomainEventToCollection()
    {
        // Arrange
        IDomainEvent domainEvent = new Mock<IDomainEvent>().Object;

        // Act
        TestCandidate.AddDomainEvent(domainEvent: domainEvent);

        // Assert
        TestCandidate.GetDomainEvents().ShouldContain(expected: domainEvent);
    }

    [Test]
    public void AddDomainEvent_WhenCalled_AddsDomainEventToCollectionInOrder()
    {
        // Arrange
        IDomainEvent domainEvent1 = new Mock<IDomainEvent>().Object;
        IDomainEvent domainEvent2 = new Mock<IDomainEvent>().Object;

        // Act
        TestCandidate.AddDomainEvent(domainEvent: domainEvent1);
        TestCandidate.AddDomainEvent(domainEvent: domainEvent2);

        // Assert
        TestCandidate.GetDomainEvents().ShouldContainInOrder(domainEvent1, domainEvent2);
    }
}