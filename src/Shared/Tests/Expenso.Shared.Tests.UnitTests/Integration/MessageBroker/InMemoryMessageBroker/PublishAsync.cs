using Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.InMemoryMessageBroker;

[TestFixture]
internal sealed class PublishAsync : MessageBrokerTestBase
{
    [Test]
    public async Task Should_PublishMessage()
    {
        // Arrange
        // Act
        await TestCandidate.PublishAsync(
            @event: new TestIntegrationEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
                MessageId: TestIntegrationEventDataSamples.SampleId,
                Payload: TestIntegrationEventDataSamples.SampleName), cancellationToken: It.IsAny<CancellationToken>());

        // Assert - See TestIntegrationEventHandler.cs
    }
}