using Expenso.Shared.Tests.UnitTests.MessageBroker.TestData;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.InMemoryMessageBroker;

internal sealed class PublishAsync : MessageBrokerTestBase
{
    [Test]
    public async Task Should_PublishMessage()
    {
        // Arrange
        // Act
        await TestCandidate.PublishAsync(TestIntegrationEventDataSamples.Sample);

        // Assert - See TestIntegrationEventHandler.cs
    }
}