using Expenso.Shared.Integration.Events;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

internal sealed record TestIntegrationEvent(Guid MessageId, string Payload) : IIntegrationEvent;