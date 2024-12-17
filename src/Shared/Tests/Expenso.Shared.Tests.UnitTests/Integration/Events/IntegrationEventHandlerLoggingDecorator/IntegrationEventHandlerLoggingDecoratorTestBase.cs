using Expenso.Shared.Integration.Events;
using Expenso.Shared.Integration.Events.Logging;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Integration.MessageBroker.TestData;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Integration.Events.IntegrationEventHandlerLoggingDecorator;

[TestFixture]
internal abstract class
    IntegrationEventHandlerLoggingDecoratorTestBase : TestBase<
    IntegrationEventHandlerLoggingDecorator<TestIntegrationEvent>>
{
    [SetUp]
    public void Setup()
    {
        _testIntegrationEvent = new TestIntegrationEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            MessageId: Guid.NewGuid(), Payload: "JYi9R7e7v2Qor");

        _loggerMock = new Mock<ILoggerService<IntegrationEventHandlerLoggingDecorator<TestIntegrationEvent>>>();
        _integrationEventHandlerMock = new Mock<IIntegrationEventHandler<TestIntegrationEvent>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new IntegrationEventHandlerLoggingDecorator<TestIntegrationEvent>(logger: _loggerMock.Object,
            decorated: _integrationEventHandlerMock.Object, serializer: _serializerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _loggerMock.Reset();
        _integrationEventHandlerMock.Reset();
        _serializerMock.Reset();
        _testIntegrationEvent = null!;
        _loggerMock = null!;
        _integrationEventHandlerMock = null!;
        _serializerMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IIntegrationEventHandler<TestIntegrationEvent>> _integrationEventHandlerMock = null!;
    protected Mock<ILoggerService<IntegrationEventHandlerLoggingDecorator<TestIntegrationEvent>>> _loggerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestIntegrationEvent _testIntegrationEvent = null!;
}