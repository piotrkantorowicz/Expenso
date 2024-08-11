namespace Expenso.Shared.System.Types.Clock;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}