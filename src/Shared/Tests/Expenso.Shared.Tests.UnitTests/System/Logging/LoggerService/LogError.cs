using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

internal sealed class LogError : LoggerServiceTestBase
{
    [Test]
    public void Should_LogErrorLevel()
    {
        // Arrange
        const string message = "Test error message";
        EventId eventId = new(id: 1004, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();
        Exception exception = new(message: "Test exception");

        // Act
        TestCandidate.LogError(eventId: eventId, message: message, exception: exception,
            messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Error, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}