using Expenso.Shared.System.Logging;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.Domain.Events.DomainEventHandlerLoggingDecorator;

[TestFixture]
internal sealed class HandleAsync : DomainEventHandlerLoggingDecoratorTestBase
{
    [Test]
    public async Task Should_LogInformation_When_NoExceptionOccurred()
    {
        // Arrange
        // Act
        await TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.DomainEventExecuting, It.IsAny<string>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.DomainEventExecuted, It.IsAny<string>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }

    [Test]
    public async Task Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _domainEventHandlerMock
            .Setup(expression: x => x.HandleAsync(_testDomainEvent, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception: new Exception(message: "Intentional thrown to test error logging."));

        // Act
        Func<Task> action = async () =>
            await TestCandidate.HandleAsync(@event: _testDomainEvent, cancellationToken: CancellationToken.None);

        // Assert
        await action
            .Should()
            .ThrowAsync<Exception>()
            .WithMessage(expectedWildcardPattern: "Intentional thrown to test error logging.");

        _loggerMock.Verify(
            expression: x => x.LogInfo(LoggingUtils.DomainEventExecuting, It.IsAny<string>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.LogError(LoggingUtils.UnexpectedError, It.IsAny<string>(), It.IsAny<Exception>(),
                It.IsAny<IMessageContext?>(), It.IsAny<object?[]>()), times: Times.Once);
    }
}