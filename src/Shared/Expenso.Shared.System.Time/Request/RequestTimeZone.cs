namespace Expenso.Shared.System.Time.Request;

public sealed class RequestTimeZone
{
    public RequestTimeZone(string name)
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById(id: name);
    }

    public RequestTimeZone(TimeZoneInfo timeZoneInfo)
    {
        TimeZone = timeZoneInfo ?? throw new ArgumentNullException(paramName: nameof(timeZoneInfo));
    }

    public TimeZoneInfo TimeZone { get; }
}