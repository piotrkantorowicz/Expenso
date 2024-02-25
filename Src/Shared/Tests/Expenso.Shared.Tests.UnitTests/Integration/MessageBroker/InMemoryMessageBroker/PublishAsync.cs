using Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.InMemoryMessageBroker;

internal sealed class PublishAsync : MessageBrokerTestBase
{
    [Test]
    public async Task Should_PublishMessage()
    {
        // Arrange
        // Act
        await TestCandidate.PublishAsync(
            new TestIntegrationEvent(MessageContextFactoryMock.Object.Current(),
                TestIntegrationEventDataSamples.SampleId, TestIntegrationEventDataSamples.SampleName),
            It.IsAny<CancellationToken>());

        // Assert - See TestIntegrationEventHandler.cs
    }
}