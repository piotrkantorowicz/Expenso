using Microsoft.Extensions.Logging;

using Moq;

namespace Expenso.Shared.Tests.Utils.UnitTests;

public static class LoggerAssertions
{
    public static void VerifyLog<T>(this Mock<ILogger<T>> loggerMock, LogLevel? logLevel = null,
        EventId? eventId = null, string? message = null, Exception? exception = null, Times? times = null)
    {
        logLevel ??= It.IsAny<LogLevel>();
        eventId ??= It.IsAny<EventId>();
        exception ??= It.IsAny<Exception>();
        times ??= Times.Once();

        if (message is null)
        {
            loggerMock.Verify(
                expression: x => x.Log(logLevel.Value, eventId.Value, It.Is<It.IsAnyType>((v, t) => true), exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: times.Value);
        }
        else
        {
            loggerMock.Verify(
                expression: x => x.Log(logLevel.Value, eventId.Value,
                    It.Is<It.IsAnyType>((v, _) => v.ToString()!.StartsWith(message)), exception,
                    It.IsAny<Func<It.IsAnyType, Exception, string>>()!), times: times.Value);
        }
    }
}