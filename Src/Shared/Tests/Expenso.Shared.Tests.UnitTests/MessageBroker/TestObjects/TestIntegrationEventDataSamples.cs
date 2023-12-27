namespace Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

internal static class TestIntegrationEventDataSamples
{
    public static TestIntegrationEvent Sample => new(Guid.NewGuid(), "Test Payload");
}