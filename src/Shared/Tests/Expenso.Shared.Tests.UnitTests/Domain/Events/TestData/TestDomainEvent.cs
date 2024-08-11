using Expenso.Shared.Domain.Types.Events;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

internal sealed record TestDomainEvent(IMessageContext MessageContext, Guid Id, string Name) : IDomainEvent;