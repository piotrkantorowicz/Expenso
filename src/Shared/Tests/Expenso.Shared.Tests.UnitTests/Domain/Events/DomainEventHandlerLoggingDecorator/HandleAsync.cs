using Expenso.Shared.System.Logging;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandlerLoggingDecorator;

internal sealed class HandleAsync : DomainEventHandlerLoggingDecoratorTestBase
{
    [Test]
    public async Task Should_LogInformation_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.DomainEventExecuting);
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.DomainEventExecuted);
    }

    [Test]
    public void Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _domainEventHandlerMock
            .Setup(expression: x => x.HandleAsync(_testDomainEvent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging"));

        // Act
        Exception? exception = Assert.ThrowsAsync<Exception>(code: () =>
            TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "Intentional thrown to test error logging");
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.DomainEventExecuting);

        _loggerMock.VerifyLog(logLevel: LogLevel.Error, eventId: LoggingUtils.UnexpectedException,
            exception: exception);
    }
}