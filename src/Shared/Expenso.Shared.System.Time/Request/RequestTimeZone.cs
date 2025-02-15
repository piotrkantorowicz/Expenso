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

    public static implicit operator string(RequestTimeZone requestTimeZoneFormat)
    {
        return requestTimeZoneFormat.TimeZone.Id;
    }

    public static implicit operator RequestTimeZone(string requestTimeZone)
    {
        return new RequestTimeZone(name: requestTimeZone);
    }
}