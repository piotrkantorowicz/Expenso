using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly string _format;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public NullableDateTimeConverter(Func<RequestTimeZone> requestTimeZone, string format)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _format = format ?? throw new ArgumentNullException(paramName: nameof(format));
    }

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (value is null)
        {
            return null;
        }

        if (DateTime.TryParse(s: value, result: out DateTime parsedDateTime))
        {
            return parsedDateTime;
        }

        return null;
    }

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        ArgumentNullException.ThrowIfNull(argument: writer);

        if (value.HasValue is false)
        {
            return;
        }

        string dateString = value.Value.ToString(format: _format, provider: CultureInfo.InvariantCulture);

        if (!DateTime.TryParseExact(s: dateString, format: _format, provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None, result: out DateTime parsedDateTime))
        {
            return;
        }

        TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;
        DateTime convertTime = TimeZoneInfo.ConvertTime(dateTime: parsedDateTime, destinationTimeZone: timeZone);
        writer.WriteStringValue(value: convertTime);
        writer.Flush();
    }
}