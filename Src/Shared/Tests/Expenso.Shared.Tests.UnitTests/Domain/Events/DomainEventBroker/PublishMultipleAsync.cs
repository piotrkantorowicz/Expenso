using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

internal sealed class PublishMultipleAsync : DomainEventBrokerTestBase
{
    [Test]
    public void Should_PublishEvent()
    {
        // Arrange
        ICollection<TestDomainEvent> domainEvents = [];

        for (int i = 0; i < 5; i++)
        {
            Guid testDomainEventId = Guid.NewGuid();

            TestDomainEvent testDomainEvent = new(MessageContext: MessageContextFactoryMock.Object.Current(),
                Id: testDomainEventId, Name: "UsWNuYtfQTtvYR");

            domainEvents.Add(item: testDomainEvent);
        }

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () =>
            TestCandidate.PublishMultipleAsync(events: domainEvents, cancellationToken: default));
    }
}