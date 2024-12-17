using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string _format;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeConverter(Func<RequestTimeZone> requestTimeZone, string format)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _format = format ?? throw new ArgumentNullException(paramName: nameof(format));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (value == null)
        {
            return default;
        }

        return DateTime.TryParse(s: value, result: out DateTime parsedDateTime) ? parsedDateTime : default;
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(argument: writer);
        string dateString = value.ToString(format: _format, provider: CultureInfo.InvariantCulture);

        if (!DateTime.TryParseExact(s: dateString, format: _format, provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None, result: out DateTime parsedDateTime))
        {
            return;
        }

        TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;
        DateTime convertTime = TimeZoneInfo.ConvertTime(dateTime: parsedDateTime, destinationTimeZone: timeZone);
        writer.WriteStringValue(value: convertTime);
    }
}