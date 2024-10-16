using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

[TestFixture]
internal sealed class PublishMultipleAsync : DomainEventBrokerTestBase
{
    [Test]
    public async Task Should_PublishEvent()
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
        Func<Task> action = async () =>
            await TestCandidate.PublishMultipleAsync(events: domainEvents, cancellationToken: default);

        // Assert
        await action.Should().NotThrowAsync();
    }
}