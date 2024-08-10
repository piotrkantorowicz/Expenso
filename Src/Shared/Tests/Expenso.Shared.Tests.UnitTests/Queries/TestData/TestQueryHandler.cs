using Expenso.Shared.Queries;

namespace Expenso.Shared.Tests.UnitTests.Queries.TestData;

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, TestResponse>
{
    public async Task<TestResponse?> HandleAsync(TestQuery query, CancellationToken cancellationToken)
    {
        TestResponse response = new(Id: query.Id, Name: "vWdGYZaiMz9cex");

        return await Task.FromResult(result: response);
    }
}