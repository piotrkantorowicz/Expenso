using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Logging;
using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;
using Expenso.Shared.Tests.Utils.UnitTests;

using Moq;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandlerLoggingDecorator;

[TestFixture]
internal abstract class
    DomainEventHandlerLoggingDecoratorTestBase : TestBase<DomainEventHandlerLoggingDecorator<TestDomainEvent>>
{
    [SetUp]
    public void Setup()
    {
        _testDomainEvent = new TestDomainEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: Guid.CreateVersion7(), Name: "JYi9R7e7v2Qor");

        _loggerMock = new Mock<ILoggerService<DomainEventHandlerLoggingDecorator<TestDomainEvent>>>();
        _domainEventHandlerMock = new Mock<IDomainEventHandler<TestDomainEvent>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new DomainEventHandlerLoggingDecorator<TestDomainEvent>(logger: _loggerMock.Object,
            decorated: _domainEventHandlerMock.Object, serializer: _serializerMock.Object);
    }

    [TearDown]
    public void TearDown()
    {
        _loggerMock.Reset();
        _domainEventHandlerMock.Reset();
        _serializerMock.Reset();
        _testDomainEvent = null!;
        _loggerMock = null!;
        _domainEventHandlerMock = null!;
        _serializerMock = null!;
        TestCandidate = null!;
    }

    protected Mock<IDomainEventHandler<TestDomainEvent>> _domainEventHandlerMock = null!;
    protected Mock<ILoggerService<DomainEventHandlerLoggingDecorator<TestDomainEvent>>> _loggerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestDomainEvent _testDomainEvent = null!;
}