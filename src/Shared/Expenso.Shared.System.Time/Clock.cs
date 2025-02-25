using Expenso.Shared.System.Time.Constants;

namespace Expenso.Shared.System.Time;

// Public, because it has to be registered in DI container
// and its intance should be created manually
public sealed class Clock : IClock, ITimeZoneClock
{
    private readonly AsyncLocal<string> _dateTimeFormat = new();
    private readonly AsyncLocal<string> _dateTimeOffsetFormat = new();
    private readonly AsyncLocal<TimeZoneInfo> _timeZone = new();

    public DateTimeOffset UtcNow => DateTimeOffset.UtcNow;

    public DateTimeOffset Now => _timeZone.Value != null
        ? TimeZoneInfo.ConvertTime(dateTimeOffset: DateTimeOffset.UtcNow, destinationTimeZone: _timeZone.Value)
        : DateTimeOffset.UtcNow;

    public string DateTimeFormat => _dateTimeFormat.Value ?? DateTimeFormats.Iso8601;

    public string DateTimeOffsetFormat => _dateTimeOffsetFormat.Value ?? DateTimeFormats.Iso8601TimeZone;

    public TimeZoneInfo TimeZone => _timeZone.Value ?? TimeZoneInfo.Utc;

    public void SetTimeZone(TimeZoneInfo? timeZone)
    {
        _timeZone.Value = timeZone ?? TimeZoneInfo.FindSystemTimeZoneById(id: TimeZoneIds.Utc);
    }

    public void SetDateTimeFormat(string? dateTimeFormat)
    {
        _dateTimeFormat.Value = dateTimeFormat ?? DateTimeFormats.Iso8601;
    }

    public void SetDateTimeOffsetFormat(string? dateTimeOffsetFormat)
    {
        _dateTimeOffsetFormat.Value = dateTimeOffsetFormat ?? DateTimeFormats.Iso8601TimeZone;
    }
}