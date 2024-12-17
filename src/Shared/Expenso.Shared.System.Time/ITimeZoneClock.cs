namespace Expenso.Shared.System.Time;

public interface ITimeZoneClock
{
    DateTimeOffset Now { get; }

    TimeZoneInfo TimeZone { get; }

    void SetTimeZone(TimeZoneInfo? timeZone);
}