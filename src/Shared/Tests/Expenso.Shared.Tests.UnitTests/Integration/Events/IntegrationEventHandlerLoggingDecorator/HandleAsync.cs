using Expenso.Shared.System.Logging;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Integration.Events.IntegrationEventHandlerLoggingDecorator;

internal sealed class HandleAsync : IntegrationEventHandlerLoggingDecoratorTestBase
{
    [Test]
    public async Task Should_LogInformation_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(@event: _testIntegrationEvent,
            cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.IntegrationEventExecuting);
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.IntegrationEventExecuted);
    }

    [Test]
    public void Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _integrationEventHandlerMock
            .Setup(expression: x => x.HandleAsync(_testIntegrationEvent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging"));

        // Act
        Exception? exception = Assert.ThrowsAsync<Exception>(code: () =>
            TestCandidate.HandleAsync(@event: _testIntegrationEvent, cancellationToken: CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "Intentional thrown to test error logging");
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.IntegrationEventExecuting);

        _loggerMock.VerifyLog(logLevel: LogLevel.Error, eventId: LoggingUtils.UnexpectedException,
            exception: exception);
    }
}