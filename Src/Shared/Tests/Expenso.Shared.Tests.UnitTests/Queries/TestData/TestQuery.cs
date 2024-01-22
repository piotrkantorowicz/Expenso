using Expenso.Shared.Queries;
using Expenso.Shared.Tests.UnitTests.Queries.QueryHandlers;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed record TestQuery(Guid Id) : IQuery<TestResponse>;