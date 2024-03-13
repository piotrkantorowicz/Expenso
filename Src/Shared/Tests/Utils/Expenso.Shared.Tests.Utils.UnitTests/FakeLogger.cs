using Microsoft.Extensions.Logging;

namespace Expenso.Shared.Tests.Utils.UnitTests;

public sealed class InMemoryFakeLogger<T> : ILogger<T>
{
    public LogLevel Level { get; private set; }

    public Exception? Ex { get; private set; }

    public string? Message { get; private set; }

    public IDisposable BeginScope<TState>(TState state) where TState : notnull
    {
        return null!;
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return true;
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception, string> formatter)
    {
        Level = logLevel;
        Message = state?.ToString();
        Ex = exception;
    }
}