namespace Expenso.Shared.System.Time;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}