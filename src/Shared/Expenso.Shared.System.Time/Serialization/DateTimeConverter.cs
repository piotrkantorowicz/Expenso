using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class DateTimeConverter : JsonConverter<DateTime>
{
    private readonly string[] _supportedFormats;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public DateTimeConverter(Func<RequestTimeZone> requestTimeZone, string[] supportedFormats)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _supportedFormats = supportedFormats ?? throw new ArgumentNullException(paramName: nameof(supportedFormats));

        if (supportedFormats.Length == 0)
        {
            throw new ArgumentException(message: "At least one format must be provided",
                paramName: nameof(supportedFormats));
        }

        _supportedFormats = supportedFormats.ToArray();
    }

    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        if (!DateTime.TryParseExact(s: value, format: _supportedFormats[0], provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None, result: out DateTime dateTime))
        {
            throw new JsonException(
                message: $"DateTime string '{value}' does not match expected format '{_supportedFormats[0]}'");
        }

        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

        return TimeZoneInfo.ConvertTime(dateTime: dateTime, sourceTimeZone: timeZone,
            destinationTimeZone: TimeZoneInfo.Utc);
    }

    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    {
        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

        try
        {
            DateTime convertTime = TimeZoneInfo.ConvertTime(dateTime: value, sourceTimeZone: TimeZoneInfo.Utc,
                destinationTimeZone: timeZone);

            writer.WriteStringValue(value: convertTime.ToString(format: _supportedFormats[0],
                provider: CultureInfo.InvariantCulture));
        }
        catch (Exception ex)
        {
            throw new JsonException(message: "Failed to convert DateTime to the specified timezone",
                innerException: ex);
        }
    }
}