using Expenso.Shared.Domain.Types.Events;

namespace Expenso.Shared.Tests.UnitTests.Domain.TestData;

internal sealed record TestDomainEvent(Guid Id, string Name) : IDomainEvent;