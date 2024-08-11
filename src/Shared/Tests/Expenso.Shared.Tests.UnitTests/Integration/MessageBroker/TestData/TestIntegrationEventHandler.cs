using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

internal sealed class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public Task HandleAsync(TestIntegrationEvent @event, CancellationToken cancellationToken)
    {
        AssertIncomingEvent(@event: @event);

        return Task.CompletedTask;
    }

    private static void AssertIncomingEvent(TestIntegrationEvent @event)
    {
        @event.Should().NotBeNull();
        @event.Should().BeOfType<TestIntegrationEvent>();
        @event.MessageId.Should().Be(expected: TestIntegrationEventDataSamples.SampleId);
        @event.Payload.Should().Be(expected: TestIntegrationEventDataSamples.SampleName);
    }
}