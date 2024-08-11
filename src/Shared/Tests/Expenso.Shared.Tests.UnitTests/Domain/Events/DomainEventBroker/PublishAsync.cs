using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

internal sealed class PublishAsync : DomainEventBrokerTestBase
{
    [Test]
    public void Should_PublishDomainEvent()
    {
        // Arrange
        Guid testDomainEventId = Guid.NewGuid();

        TestDomainEvent testDomainEvent = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: testDomainEventId, Name: "UsWNuYtfQTtvYR");

        // Act
        // Assert
        Assert.DoesNotThrowAsync(code: () =>
            TestCandidate.PublishAsync(@event: testDomainEvent, cancellationToken: default));
    }
}