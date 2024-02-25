using Expenso.Shared.Queries;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, TestResponse>
{
    public async Task<TestResponse?> HandleAsync(TestQuery query, CancellationToken cancellationToken)
    {
        TestResponse response = new(query.Id, "vWdGYZaiMz9cex");

        return await Task.FromResult(response);
    }
}