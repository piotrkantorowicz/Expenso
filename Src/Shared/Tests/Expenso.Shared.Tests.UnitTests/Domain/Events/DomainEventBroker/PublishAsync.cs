using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

internal sealed class PublishAsync : DomainEventBrokerTestBase
{
    [Test]
    public void Should_PublishDomainEvent()
    {
        // Arrange
        Guid testDomainEventId = Guid.NewGuid();

        TestDomainEvent testDomainEvent = new(MessageContextFactoryMock.Object.Current(),
            testDomainEventId, "UsWNuYtfQTtvYR");

        // Act
        // Assert
        Assert.DoesNotThrowAsync(() => TestCandidate.PublishAsync(testDomainEvent));
    }
}