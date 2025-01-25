using Expenso.Shared.Domain.Types.Events;

using Moq;

using NUnit.Framework;

using Shouldly;

namespace Expenso.BudgetSharing.Tests.UnitTests.Domain.Shared.Base;

[TestFixture]
internal sealed class GetDomainEvents : DomainEventsSourceTestBase
{
    [Test]
    public void GetDomainEvents_WhenCalled_ReturnsEmptyCollection()
    {
        // Arrange
        // Act
        IReadOnlyCollection<IDomainEvent> result = TestCandidate.GetDomainEvents();

        // Assert
        result.ShouldBeEmpty();
    }

    [Test]
    public void GetDomainEvents_WhenCalled_ReturnsDomainEventsAndClearsThem()
    {
        // Arrange
        IDomainEvent domainEvent = new Mock<IDomainEvent>().Object;
        TestCandidate.AddDomainEvent(domainEvent: domainEvent);

        // Act
        IReadOnlyCollection<IDomainEvent> result = TestCandidate.GetDomainEvents();

        // Assert
        result.ShouldContain(expected: domainEvent);
        TestCandidate.GetDomainEvents().ShouldBeEmpty();
    }
}