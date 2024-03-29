using Expenso.Shared.Queries;
using Expenso.Shared.Queries.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Queries.TestData;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandlerLoggingDecorator;

internal abstract class
    QueryHandlerLoggingDecoratorTestBase : TestBase<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>
{
    protected Mock<ILogger<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>> _loggerMock = null!;
    protected Mock<IQueryHandler<TestQuery, TestResponse>> _queryHandlerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestQuery _testQuery = null!;

    [SetUp]
    protected void Setup()
    {
        _testQuery = new TestQuery(MessageContextFactoryMock.Object.Current(), Guid.NewGuid());
        _loggerMock = new Mock<ILogger<QueryHandlerLoggingDecorator<TestQuery, TestResponse>>>();
        _queryHandlerMock = new Mock<IQueryHandler<TestQuery, TestResponse>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new QueryHandlerLoggingDecorator<TestQuery, TestResponse>(_loggerMock.Object,
            _queryHandlerMock.Object, _serializerMock.Object);
    }
}