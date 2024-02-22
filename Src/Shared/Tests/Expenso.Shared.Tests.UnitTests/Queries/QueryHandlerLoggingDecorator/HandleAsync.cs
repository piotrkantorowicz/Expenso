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
        await TestCandidate.HandleAsync(_testQuery, It.IsAny<CancellationToken>());

        // Assert
        _loggerMock.Verify(
            x => x.Log(LogLevel.Information, LoggingUtils.QueryExecuting, It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        _loggerMock.Verify(
            x => x.Log(LogLevel.Information, LoggingUtils.QueryExecuted, It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }

    [Test]
    public void Should_LogError_When_ExceptionOccurred()
    {
        // Arrange
        _queryHandlerMock
            .Setup(x => x.HandleAsync(_testQuery, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Intentional thrown to test error logging"));

        // Act
        Exception? exception =
            Assert.ThrowsAsync<Exception>(() => TestCandidate.HandleAsync(_testQuery, CancellationToken.None));

        // Assert
        exception.Should().NotBeNull();
        exception?.Message.Should().Be("Intentional thrown to test error logging");

        _loggerMock.Verify(
            x => x.Log(LogLevel.Information, LoggingUtils.QueryExecuting, It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);

        _loggerMock.Verify(
            x => x.Log(LogLevel.Error, LoggingUtils.UnexpectedException, It.Is<It.IsAnyType>((v, t) => true),
                It.IsAny<Exception>(), It.IsAny<Func<It.IsAnyType, Exception, string>>()!), Times.Once);
    }
}