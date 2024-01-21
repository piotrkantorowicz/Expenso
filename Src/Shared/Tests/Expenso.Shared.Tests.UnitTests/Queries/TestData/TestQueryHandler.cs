using Expenso.Shared.Queries;
using Expenso.Shared.Tests.UnitTests.Queries.QueryHandlers;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, TestResponse>
{
    public async Task<TestResponse?> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
    {
        TestResponse response = new(query.Id, "vWdGYZaiMz9cex");

        return await Task.FromResult(response);
    }
}