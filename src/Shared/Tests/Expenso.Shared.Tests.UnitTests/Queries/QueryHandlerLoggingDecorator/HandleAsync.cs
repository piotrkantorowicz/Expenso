using Expenso.Shared.System.Logging;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Queries.QueryHandlerLoggingDecorator;

internal sealed class HandleAsync : QueryHandlerLoggingDecoratorTestBase
{
    [Test]
    public async Task Should_LogInformation_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(query: _testQuery, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.QueryExecuting);
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.QueryExecuted);
    }

    [Test]
    public void Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _queryHandlerMock
            .Setup(expression: x => x.HandleAsync(_testQuery, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging"));

        // Act
        Exception? exception = Assert.ThrowsAsync<Exception>(code: () =>
            TestCandidate.HandleAsync(query: _testQuery, cancellationToken: CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "Intentional thrown to test error logging");
        _loggerMock.VerifyLog(logLevel: LogLevel.Information, eventId: LoggingUtils.QueryExecuting);

        _loggerMock.VerifyLog(logLevel: LogLevel.Error, eventId: LoggingUtils.UnexpectedException,
            exception: exception);
    }
}