using Expenso.Shared.Domain.Events;
using Expenso.Shared.Domain.Events.Logging;
using Expenso.Shared.System.Serialization;
using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandlerLoggingDecorator;

internal abstract class
    DomainEventHandlerLoggingDecoratorTestBase : TestBase<DomainEventHandlerLoggingDecorator<TestDomainEvent>>
{
    protected Mock<IDomainEventHandler<TestDomainEvent>> _domainEventHandlerMock = null!;
    protected Mock<ILogger<DomainEventHandlerLoggingDecorator<TestDomainEvent>>> _loggerMock = null!;
    private Mock<ISerializer> _serializerMock = null!;
    protected TestDomainEvent _testDomainEvent = null!;

    [SetUp]
    protected void Setup()
    {
        _testDomainEvent =
            new TestDomainEvent(MessageContextFactoryMock.Object.Current(), Guid.NewGuid(), "JYi9R7e7v2Qor");

        _loggerMock = new Mock<ILogger<DomainEventHandlerLoggingDecorator<TestDomainEvent>>>();
        _domainEventHandlerMock = new Mock<IDomainEventHandler<TestDomainEvent>>();
        _serializerMock = new Mock<ISerializer>();

        TestCandidate = new DomainEventHandlerLoggingDecorator<TestDomainEvent>(_loggerMock.Object,
            _domainEventHandlerMock.Object, _serializerMock.Object);
    }
}