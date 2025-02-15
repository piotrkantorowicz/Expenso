namespace Expenso.Shared.System.Time;

public interface ITimeZoneClock
{
    DateTimeOffset Now { get; }

    TimeZoneInfo TimeZone { get; }

    string DateTimeFormat { get; }

    string DateTimeOffsetFormat { get; }

    void SetTimeZone(TimeZoneInfo? timeZone);

    void SetDateTimeFormat(string? dateTimeFormat);

    void SetDateTimeOffsetFormat(string? dateTimeOffsetFormat);
}