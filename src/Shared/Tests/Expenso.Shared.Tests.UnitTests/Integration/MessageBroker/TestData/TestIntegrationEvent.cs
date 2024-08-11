using Expenso.Shared.Integration.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;

internal sealed record TestIntegrationEvent(IMessageContext MessageContext, Guid MessageId, string Payload)
    : IIntegrationEvent;