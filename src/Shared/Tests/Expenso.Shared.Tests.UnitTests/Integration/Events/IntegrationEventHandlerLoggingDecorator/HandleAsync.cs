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
        _loggerMock.Verify(
            expression: x => x.Log(LogLevel.Information, LoggingUtils.IntegrationEventExecuting,
                It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.Log(LogLevel.Information, LoggingUtils.IntegrationEventExecuted,
                It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
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

        _loggerMock.Verify(
            expression: x => x.Log(LogLevel.Information, LoggingUtils.IntegrationEventExecuting,
                It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);

        _loggerMock.Verify(
            expression: x => x.Log(LogLevel.Error, LoggingUtils.UnexpectedException,
                It.Is<It.IsAnyType>((v, t) => true), It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}