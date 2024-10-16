using Expenso.Shared.Tests.UnitTests.Queries.TestData;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

[TestFixture]
internal abstract class QueryHandlerResultTestBase : TestBase<TestQueryHandler>
{
    [SetUp]
    protected void Setup()
    {
        _testQuery = new TestQuery(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid());
        TestCandidate = new TestQueryHandler();
    }

    protected TestQuery _testQuery = null!;
}