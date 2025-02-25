namespace Expenso.Shared.System.Time.Request;

public class RequestDateTimeOffsetFormat
{
    public RequestDateTimeOffsetFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(value: format))
        {
            throw new ArgumentException(message: "Format cannot be null or empty", paramName: nameof(format));
        }

        Format = format;
    }

    public string Format { get; }

    public string FormatDate(DateTimeOffset dateTimeOffset)
    {
        return dateTimeOffset.ToString(format: Format);
    }

    public static implicit operator string(RequestDateTimeOffsetFormat format)
    {
        return format.Format;
    }

    public static implicit operator RequestDateTimeOffsetFormat(string format)
    {
        return new RequestDateTimeOffsetFormat(format: format);
    }
}