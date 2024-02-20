using Expenso.Shared.Queries;
using Expenso.Shared.System.Types.Messages.Interfaces;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed record TestQuery(IMessageContext MessageContext, Guid Id) : IQuery<TestResponse>;