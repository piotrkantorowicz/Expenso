using Expenso.Shared.Tests.UnitTests.Domain.TestData;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.DomainEventHandler;

internal abstract class DomainEventHandlerTestBase : TestBase<TestDomainEventHandler>
{
    protected Mock<ILogger<TestDomainEventHandler>> _loggerMock = null!;
    protected TestDomainEvent _testDomainEvent = null!;

    [SetUp]
    protected void Setup()
    {
        _testDomainEvent = new TestDomainEvent(Guid.NewGuid(), "GiKyb3G");
        _loggerMock = new Mock<ILogger<TestDomainEventHandler>>();
        TestCandidate = new TestDomainEventHandler(_loggerMock.Object);
    }
}