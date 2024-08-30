using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Logging;

public interface ILoggerService<T>
{
    void LogTrace(EventId eventId, string? message, IMessageContext? messageContext = null, params object?[] args);

    void LogDebug(EventId eventId, string? message, IMessageContext? messageContext = null, params object?[] args);

    void LogInfo(EventId eventId, string? message, IMessageContext? messageContext = null, params object?[] args);

    void LogWarning(EventId eventId, string? message, Exception? exception = null,
        IMessageContext? messageContext = null, params object?[] args);

    void LogError(EventId eventId, string? message, Exception? exception = null, IMessageContext? messageContext = null,
        params object?[] args);

    void LogCritical(EventId eventId, string? message, Exception? exception = null,
        IMessageContext? messageContext = null, params object?[] args);
}