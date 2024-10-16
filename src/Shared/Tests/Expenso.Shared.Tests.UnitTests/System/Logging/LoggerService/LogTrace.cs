using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

[TestFixture]
internal sealed class LogTrace : LoggerServiceTestBase
{
    [Test]
    public void Should_LogTraceLevel()
    {
        // Arrange
        const string message = "Test trace message";
        EventId eventId = new(id: 1000, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();

        // Act
        TestCandidate.LogTrace(eventId: eventId, message: message, messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Trace, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}