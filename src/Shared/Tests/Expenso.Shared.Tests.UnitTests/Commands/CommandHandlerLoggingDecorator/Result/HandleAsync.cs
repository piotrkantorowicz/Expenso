using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Commands.CommandHandlerLoggingDecorator.Result;

internal sealed class HandleAsync : CommandHandlerLoggingDecoratorTestBase
{
    [Test]
    public async Task Should_LogInformation_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(command: _testCommand, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        // Assert
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.CommandExecuting, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.CommandExecuted, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public void Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _commandHandlerMock
            .Setup(expression: x => x.HandleAsync(_testCommand, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging"));

        // Act
        Exception? exception = Assert.ThrowsAsync<Exception>(code: () =>
            TestCandidate.HandleAsync(command: _testCommand, cancellationToken: CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be(expected: "Intentional thrown to test error logging");

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.CommandExecuting, It.IsAny<string>(), It.IsAny<IMessageContext?>(),
                It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.UnexpectedError, It.IsAny<string>(), It.IsAny<Exception>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }
}