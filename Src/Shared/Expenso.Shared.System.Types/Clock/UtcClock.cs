namespace Expenso.Shared.System.Types.Clock;

internal sealed class UtcClock : IClock
{
    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;
}