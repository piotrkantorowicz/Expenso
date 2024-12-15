using Expenso.Shared.Tests.UnitTests.Queries.TestData;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

[TestFixture]
internal abstract class QueryHandlerResultTestBase : TestBase<TestQueryHandler>
{
    [SetUp]
    public void Setup()
    {
        _testQuery = new TestQuery(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid());
        TestCandidate = new TestQueryHandler();
    }

    [TearDown]
    public void TearDown()
    {
        _testQuery = null!;
        TestCandidate = null!;
    }

    protected TestQuery _testQuery = null!;
}