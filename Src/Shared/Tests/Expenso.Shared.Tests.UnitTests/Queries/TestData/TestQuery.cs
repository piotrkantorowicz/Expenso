using Expenso.Shared.Queries;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed record TestQuery(Guid Id) : IQuery<TestResponse>;