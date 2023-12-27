using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

internal sealed class TestIntegrationEventHandler : IIntegrationEventHandler<TestIntegrationEvent>
{
    public Task HandleAsync(TestIntegrationEvent @event, CancellationToken cancellationToken = default)
    {
        TestIntegrationEvent expectedTestIntegrationEvent = TestIntegrationEventDataSamples.Sample;
        @event.Should().NotBeNull();
        @event.Should().BeOfType<TestIntegrationEvent>();
        @event.MessageId.Should().Be(expectedTestIntegrationEvent.MessageId);
        @event.Payload.Should().Be(expectedTestIntegrationEvent.Payload);

        return Task.CompletedTask;
    }
}