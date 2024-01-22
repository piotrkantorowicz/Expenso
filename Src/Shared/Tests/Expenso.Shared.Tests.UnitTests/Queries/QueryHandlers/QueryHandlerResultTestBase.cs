using Expenso.Shared.Tests.UnitTests.Queries.TestData;

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