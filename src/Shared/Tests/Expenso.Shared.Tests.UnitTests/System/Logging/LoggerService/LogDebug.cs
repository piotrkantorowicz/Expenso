using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

[TestFixture]
internal sealed class LogDebug : LoggerServiceTestBase
{
    [Test]
    public void Should_LogDebugLevel()
    {
        // Arrange
        const string message = "Test debug message";
        EventId eventId = new(id: 1001, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();

        // Act
        TestCandidate.LogDebug(eventId: eventId, message: message, messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Debug, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}