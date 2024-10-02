using Expenso.Shared.System.Configuration.Settings.App;
using Expenso.Shared.System.Types.Messages.Interfaces;

using Microsoft.Extensions.Logging;

namespace Expenso.Shared.System.Logging;

internal sealed class LoggerService<T> : ILoggerService<T> where T : class
{
    private readonly ApplicationSettings _applicationSettings;
    private readonly ILoggerFactory _loggerFactory;

    public LoggerService(ILoggerFactory logger, ApplicationSettings applicationSettings)
    {
        _loggerFactory = logger ?? throw new ArgumentNullException(paramName: nameof(logger));

        _applicationSettings = applicationSettings ??
                               throw new ArgumentNullException(paramName: nameof(applicationSettings));
    }

    public void LogTrace(EventId eventId, string? message, IMessageContext? messageContext = null,
        params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Trace, eventId: eventId, exception: null,
            message: message, args: args);
    }

    public void LogDebug(EventId eventId, string? message, IMessageContext? messageContext = null,
        params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Debug, eventId: eventId, exception: null,
            message: message, args: args);
    }

    public void LogInfo(EventId eventId, string? message, IMessageContext? messageContext = null, params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Information, eventId: eventId, exception: null,
            message: message, args: args);
    }

    public void LogWarning(EventId eventId, string? message, Exception? exception = null,
        IMessageContext? messageContext = null, params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Warning, eventId: eventId, exception: exception,
            message: message, args: args);
    }

    public void LogError(EventId eventId, string? message, Exception? exception = null,
        IMessageContext? messageContext = null, params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Error, eventId: eventId, exception: exception,
            message: message, args: args);
    }

    public void LogCritical(EventId eventId, string? message, Exception? exception = null,
        IMessageContext? messageContext = null, params object?[] args)
    {
        Log(messageContext: messageContext, logLevel: LogLevel.Critical, eventId: eventId, exception: exception,
            message: message, args: args);
    }

    private void Log(IMessageContext? messageContext, LogLevel logLevel, EventId eventId, Exception? exception,
        string? message, params object?[] args)
    {
        ILogger<T> logger = _loggerFactory.CreateLogger<T>();
        List<KeyValuePair<string, object>> parameters = GetLogParameters(messageContext: messageContext);

        using (logger.BeginScope(state: parameters))
        {
            // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
            logger.Log(logLevel: logLevel, eventId: eventId, exception: exception, message: message, args: args);
        }
    }

    private List<KeyValuePair<string, object>> GetLogParameters(IMessageContext? messageContext)
    {
        List<KeyValuePair<string, object>> parameters = [new(key: "AppId", value: _applicationSettings.InstanceId!)];

        if (!string.IsNullOrEmpty(value: _applicationSettings.Name))
        {
            parameters.Add(item: new KeyValuePair<string, object>(key: "App", value: _applicationSettings.Name));
        }

        if (!string.IsNullOrEmpty(value: _applicationSettings.Version))
        {
            parameters.Add(item: new KeyValuePair<string, object>(key: "Version", value: _applicationSettings.Version));
        }

        if (messageContext is not null)
        {
            parameters.AddRange(collection:
            [
                new KeyValuePair<string, object>(key: "Module", value: messageContext.ModuleId ?? string.Empty),
                new KeyValuePair<string, object>(key: "MessageId", value: messageContext.MessageId),
                new KeyValuePair<string, object>(key: "CorrelationId", value: messageContext.CorrelationId),
                new KeyValuePair<string, object>(key: "RequestedBy", value: messageContext.RequestedBy),
                new KeyValuePair<string, object>(key: "Timestamp", value: messageContext.Timestamp)
            ]);
        }

        return parameters;
    }
}