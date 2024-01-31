namespace Expenso.Shared.Types.Clock;

public class UtcClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}