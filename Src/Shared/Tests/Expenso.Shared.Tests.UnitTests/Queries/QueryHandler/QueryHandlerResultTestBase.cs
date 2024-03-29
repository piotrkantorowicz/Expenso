using Expenso.Shared.Tests.UnitTests.Queries.TestData;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

internal abstract class QueryHandlerResultTestBase : TestBase<TestQueryHandler>
{
    protected TestQuery _testQuery = null!;

    [SetUp]
    protected void Setup()
    {
        _testQuery = new TestQuery(MessageContextFactoryMock.Object.Current(), Guid.NewGuid());
        TestCandidate = new TestQueryHandler();
    }
}