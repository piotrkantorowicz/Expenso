using Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.InMemoryMessageBroker;

internal sealed class PublishAsync : MessageBrokerTestBase
{
    [Test]
    public async Task Should_PublishMessage()
    {
        // Arrange
        // Act
        await TestCandidate.PublishAsync(new TestIntegrationEvent(MessageContextFactoryMock.Object.Current(),
            TestIntegrationEventDataSamples.SampleId, TestIntegrationEventDataSamples.SampleName));

        // Assert - See TestIntegrationEventHandler.cs
    }
}