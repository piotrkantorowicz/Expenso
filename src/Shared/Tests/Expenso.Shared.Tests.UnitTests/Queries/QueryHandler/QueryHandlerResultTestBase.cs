using Expenso.Shared.Tests.UnitTests.Queries.TestData;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandler;

[TestFixture]
internal abstract class QueryHandlerResultTestBase : TestBase<TestQueryHandler>
{
    [SetUp]
    public void Setup()
    {
        _testQuery = new TestQuery(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: Guid.CreateVersion7());
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