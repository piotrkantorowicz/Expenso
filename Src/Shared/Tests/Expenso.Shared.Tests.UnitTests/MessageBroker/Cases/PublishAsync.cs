using Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.Cases;

internal sealed class PublishAsync : MessageBrokerTestBase
{
    [Test]
    public async Task ShouldPublishMessage()
    {
        // Arrange
        var integrationEvent = AutoFixtureProxy.Create<TestIntegrationEvent>();

        // Act
        await TestCandidate.PublishAsync(integrationEvent);
    }

    protected override async Task AssertSubscribedEvent<TIntegrationEvent>(TIntegrationEvent integrationEvent,
        CancellationToken cancellationToken)
    {
        // Assert
        integrationEvent.Should().NotBeNull();
        integrationEvent.Should().BeOfType<TestIntegrationEvent>();
        integrationEvent.Should().BeEquivalentTo(integrationEvent);
        await Task.CompletedTask;
    }
}