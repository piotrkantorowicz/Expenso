namespace Expenso.Shared.Types.Clock;

public interface IClock
{
    DateTimeOffset UtcNow { get; }
}