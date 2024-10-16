using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.UnitTests.System.Logging.LoggerService;

[TestFixture]
internal sealed class LogInfo : LoggerServiceTestBase
{
    [Test]
    public void Should_LogInfoirmationLevel()
    {
        // Arrange
        const string message = "Test information message";
        EventId eventId = new(id: 1002, name: "TestEvent");
        IMessageContext messageContext = MessageContextFactoryMock.Object.Current();

        // Act
        TestCandidate.LogInfo(eventId: eventId, message: message, messageContext: messageContext);

        // Assert
        _loggerMock?.Verify(
            expression: x => x.Log(LogLevel.Information, eventId,
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), null,
                It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: Times.Once);
    }
}