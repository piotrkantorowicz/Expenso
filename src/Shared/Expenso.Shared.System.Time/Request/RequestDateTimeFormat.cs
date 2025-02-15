namespace Expenso.Shared.System.Time.Request;

public sealed class RequestDateTimeFormat
{
    public RequestDateTimeFormat(string format)
    {
        if (string.IsNullOrWhiteSpace(value: format))
        {
            throw new ArgumentException(message: "Format cannot be null or empty", paramName: nameof(format));
        }

        Format = format;
    }

    public string Format { get; }

    public string FormatDate(DateTime dateTime)
    {
        return dateTime.ToString(format: Format);
    }

    public static implicit operator string(RequestDateTimeFormat format)
    {
        return format.Format;
    }

    public static implicit operator RequestDateTimeFormat(string format)
    {
        return new RequestDateTimeFormat(format: format);
    }
}