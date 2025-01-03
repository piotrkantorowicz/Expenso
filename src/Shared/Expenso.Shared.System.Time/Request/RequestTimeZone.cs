namespace Expenso.Shared.System.Time.Request;

public sealed class RequestTimeZone
{
    public RequestTimeZone(string? name)
    {
        TimeZone = TimeZoneInfo.FindSystemTimeZoneById(id: name ??
                                                           throw new TimeZoneNotFoundException(
                                                               message: "The time zone name is null."));
    }

    public RequestTimeZone(TimeZoneInfo timeZoneInfo)
    {
        TimeZone = timeZoneInfo ?? throw new ArgumentNullException(paramName: nameof(timeZoneInfo));
    }

    public TimeZoneInfo TimeZone { get; }
}