using Expenso.Shared.Queries;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandlers;

internal abstract class QueryHandlerResultTestBase : TestBase<TestQueryHandler>
{
    protected TestQuery _testQuery = null!;

    [SetUp]
    protected void Setup()
    {
        _testQuery = new TestQuery(Guid.NewGuid());
        TestCandidate = new TestQueryHandler();
    }
}

internal sealed record TestQuery(Guid Id) : IQuery<TestResponse>;

internal sealed record TestResponse(Guid Id, string Name);

internal sealed class TestQueryHandler : IQueryHandler<TestQuery, TestResponse>
{
    public async Task<TestResponse?> HandleAsync(TestQuery query, CancellationToken cancellationToken = default)
    {
        TestResponse response = new TestResponse(query.Id, "vWdGYZaiMz9cex");

        return await Task.FromResult(response);
    }
}