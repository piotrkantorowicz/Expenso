using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandlerLoggingDecorator;

[TestFixture]
internal abstract class
    QueryHandlerLoggingDecoratorTestBase : TestBase<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>
{
    [SetUp]
    public void Setup()
    {
        _testQuery = new TestQuery(MessageContext: MessageContextFactoryMock.Object.Current(), Id: Guid.NewGuid());
        _loggerMock = new Mock<ILoggerService<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>>();
        _queryHandlerMock = new Mock<IQueryHandler<TestQuery, TestResponse>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new QueryHandlerLoggingDecorator<TestQuery, TestResponse>(logger: _loggerMock.Object,
            decorated: _queryHandlerMock.Object, serializer: _serializerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _loggerMock.Reset();
        _queryHandlerMock.Reset();
        _serializerMock.Reset();
        _testQuery = null!;
        _loggerMock = null!;
        _queryHandlerMock = null!;
        _serializerMock = null!;
        TestCandidate = null!;
    }

    protected Mock<ILoggerService<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>> _loggerMock = null!;
    protected Mock<IQueryHandler<TestQuery, TestResponse>> _queryHandlerMock = null!;
    protected TestQuery _testQuery = null!;
    private Mock<ISerializer> _serializerMock = null!;
}