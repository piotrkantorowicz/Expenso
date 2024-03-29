using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

internal sealed class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public Task HandleAsync(TestIntegrationEvent @event, CancellationToken cancellationToken)
    {
        AssertIncomingEvent(@event);

        return Task.CompletedTask;
    }

    private static void AssertIncomingEvent(TestIntegrationEvent @event)
    {
        @event.Should().NotBeNull();
        @event.Should().BeOfType<TestIntegrationEvent>();
        @event.MessageId.Should().Be(TestIntegrationEventDataSamples.SampleId);
        @event.Payload.Should().Be(TestIntegrationEventDataSamples.SampleName);
    }
}