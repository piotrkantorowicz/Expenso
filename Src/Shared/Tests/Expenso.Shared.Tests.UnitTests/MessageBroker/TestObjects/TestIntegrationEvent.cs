using Expenso.Shared.IntegrationEvents;

namespace Expenso.Shared.Tests.UnitTests.MessageBroker.TestObjects;

internal sealed record TestIntegrationEvent(Guid MessageId, string Payload) : IIntegrationEvent;