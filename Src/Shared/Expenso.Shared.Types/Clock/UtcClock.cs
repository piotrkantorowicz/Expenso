namespace Expenso.Shared.Types.Clock;

internal sealed class UtcClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}