using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

using NUnit.Framework;

using Shouldly;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventBroker;

[TestFixture]
internal sealed class PublishAsync : DomainEventBrokerTestBase
{
    [Test]
    public async Task Should_PublishDomainEvent()
    {
        // Arrange
        Guid testDomainEventId = Guid.CreateVersion7();

        TestDomainEvent testDomainEvent = new(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: testDomainEventId, Name: "UsWNuYtfQTtvYR");

        // Act
        Func<Task> action = async () =>
            await TestCandidate.PublishAsync(@event: testDomainEvent, cancellationToken: default);

        // Assert
        await action.ShouldNotThrowAsync();
    }
}