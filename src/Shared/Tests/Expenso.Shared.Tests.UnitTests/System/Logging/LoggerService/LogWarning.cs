using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

internal sealed class LogWarning : LoggerServiceTestBase
{
    [Test]
    public void Should_LogWarningLevel()
    {
        // Arrange
        const string message = "Test warning message";
        EventId eventId = new(id: 1003, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();
        Exception exception = new(message: "Test exception");

        // Act
        TestCandidate.LogWarning(eventId: eventId, message: message, exception: exception,
            messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Warning, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}