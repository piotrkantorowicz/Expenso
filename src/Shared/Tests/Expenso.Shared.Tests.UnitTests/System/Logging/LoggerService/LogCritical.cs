using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

[TestFixture]
internal sealed class LogCritical : LoggerServiceTestBase
{
    [Test]
    public void Should_LogCriticalLevel()
    {
        // Arrange
        const string message = "Test critical message";
        EventId eventId = new(id: 1005, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();
        Exception exception = new(message: "Test exception");

        // Act
        TestCandidate.LogCritical(eventId: eventId, message: message, exception: exception,
            messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Critical, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), exception,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}