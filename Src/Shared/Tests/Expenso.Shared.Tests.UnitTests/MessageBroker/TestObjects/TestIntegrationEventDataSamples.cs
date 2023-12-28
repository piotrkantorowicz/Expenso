namespace Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

internal static class TestIntegrationEventDataSamples
{
    public static TestIntegrationEvent Sample => new(new Guid("845da593-4af4-47d0-af54-92338eef055d"), "Test Payload");
}