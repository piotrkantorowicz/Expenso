using Expenso.Shared.Domain.Types.Events;

using FluentAssertions;

using Moq;

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
        result.Should().BeEmpty();
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
        result.Should().Contain(expected: domainEvent);
        TestCandidate.GetDomainEvents().Should().BeEmpty();
    }
}