using Expenso.Shared.System.Logging;
using Expenso.Shared.Tests.UnitTests.Domain.Events.TestData;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandler;

internal abstract class DomainEventHandlerTestBase : TestBase<TestDomainEventHandler>
{
    protected Mock<ILoggerService<TestDomainEventHandler>> _loggerMock = null!;
    protected TestDomainEvent _testDomainEvent = null!;

    [SetUp]
    protected void Setup()
    {
        _testDomainEvent = new TestDomainEvent(MessageContext: MessageContextFactoryMock.Object.Current(),
            Id: Guid.NewGuid(), Name: "GiKyb3G");

        _loggerMock = new Mock<ILoggerService<TestDomainEventHandler>>();
        TestCandidate = new TestDomainEventHandler(logger: _loggerMock.Object);
    }
}