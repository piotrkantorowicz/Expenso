using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly Func<RequestDateTimeFormat> _requestDateTimeFormat;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeConverter(Func<RequestTimeZone> requestTimeZone, Func<RequestDateTimeFormat> requestDateTimeFormat)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));

        _requestDateTimeFormat = requestDateTimeFormat ??
                                 throw new ArgumentNullException(paramName: nameof(requestDateTimeFormat));
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        RequestTimeZone requestTimeZone = _requestTimeZone();
        RequestDateTimeFormat requestDateTimeFormat = _requestDateTimeFormat();

        if (!DateTime.TryParseExact(s: value, format: requestDateTimeFormat.Format,
                provider: CultureInfo.InvariantCulture, style: DateTimeStyles.None, result: out DateTime dateTime))
        {
            throw new JsonException(
                message: $"DateTime string '{value}' does not match expected format '{requestDateTimeFormat.Format}'");
        }

        return TimeZoneInfo.ConvertTime(dateTime: dateTime, sourceTimeZone: requestTimeZone.TimeZone,
            destinationTimeZone: TimeZoneInfo.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        try
        {
            DateTime convertTime = TimeZoneInfo.ConvertTime(dateTime: value, sourceTimeZone: TimeZoneInfo.Utc,
                destinationTimeZone: _requestTimeZone().TimeZone);

            writer.WriteStringValue(value: convertTime.ToString(format: _requestDateTimeFormat().Format,
                provider: CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            throw new JsonException(message: "Failed to convert DateTime to the specified timezone",
                innerException: ex);
        }
    }
}