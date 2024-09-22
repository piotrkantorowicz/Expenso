using Expenso.Shared.System.Logging;

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
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.IntegrationEventExecuting, It.IsAny<string>(),
                _testIntegrationEvent.MessageContext, It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.IntegrationEventExecuted, It.IsAny<string>(),
                _testIntegrationEvent.MessageContext, It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public async Task Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _integrationEventHandlerMock
            .Setup(expression: x => x.HandleAsync(_testIntegrationEvent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging."));

        // Act
        Func<Task> action = async () => await TestCandidate.HandleAsync(
            @event: _testIntegrationEvent, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(expectedWildcardPattern: "Intentional thrown to test error logging.");

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.IntegrationEventExecuting, It.IsAny<string>(),
                _testIntegrationEvent.MessageContext, It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.UnexpectedError, It.IsAny<string>(), It.IsAny<Exception>(),
                _testIntegrationEvent.MessageContext, It.IsAny<object?[]>()), times: Times.Once);
    }
}