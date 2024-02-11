using Expenso.Shared.Tests.UnitTests.Domain.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.DomainEventBroker;

internal sealed class PublishAsync : DomainEventBrokerTestBase
{
    [Test]
    public void Should_PublishDomainEvent()
    {
        // Arrange
        Guid testDomainEventId = Guid.NewGuid();
        TestDomainEvent testDomainEvent = new(testDomainEventId, "UsWNuYtfQTtvYR");

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => TestCandidate.PublishAsync(testDomainEvent));
    }
}