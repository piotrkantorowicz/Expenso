using Expenso.Shared.Integration.Events;

using Shouldly;

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
        @event.ShouldNotBeNull();
        @event.ShouldBeOfType<TestIntegrationEvent>();
        @event.MessageId.ShouldBe(expected: TestIntegrationEventDataSamples.SampleId);
        @event.Payload.ShouldBe(expected: TestIntegrationEventDataSamples.SampleName);
    }
}