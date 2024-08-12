using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandler;

internal sealed class HandleAsync : DomainEventHandlerTestBase
{
    [Test]
    public async Task Should_HandleDomainEvent()
    {
        // Arrange
        _loggerMock.Setup(expression: x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));

        // Act
        await TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.VerifyLog(logLevel: LogLevel.Information);
    }
}