using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

internal sealed class PublishAsync : DomainEventBrokerTestBase
{
    [Test]
    public async Task Should_PublishDomainEvent()
    {
        // Arrange
        Guid testDomainEventId = Guid.NewGuid();

        TestDomainEvent testDomainEvent = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: testDomainEventId, Name: "UsWNuYtfQTtvYR");

        // Act
        Func<Task> action = async () =>
            await TestCandidate.PublishAsync(@event: testDomainEvent, cancellationToken: default);

        // Assert
        await action.Should().NotThrowAsync();
    }
}