using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandler;

internal sealed class HandleAsync : DomainEventHandlerTestBase
{
    [Test]
    public async Task Should_HandleDomainEvent()
    {
        // Arrange
        _loggerMock.Setup(x => x.Log(It.IsAny<LogLevel>(), It.IsAny<EventId>(), It.IsAny<It.IsAnyType>(),
            It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!));

        // Act
        await TestCandidate.HandleAsync(_testDomainEvent, It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.Verify(
            x => x.Log(LogLevel.Information, It.IsAny<EventId>(), It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}