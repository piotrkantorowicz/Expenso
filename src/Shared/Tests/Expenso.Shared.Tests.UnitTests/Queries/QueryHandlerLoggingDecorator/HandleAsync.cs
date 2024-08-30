using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

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
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.QueryExecuting, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.QueryExecuted, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);
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

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.QueryExecuting, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.UnexpectedError, It.IsAny<string>(), It.IsAny<Exception>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }
}