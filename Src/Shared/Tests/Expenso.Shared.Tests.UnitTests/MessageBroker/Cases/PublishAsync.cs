using Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.Cases;

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