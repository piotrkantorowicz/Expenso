using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class NullableDateTimeConverter : JsonConverter<DateTime?>
{
    private readonly string[] _supportedFormats;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public NullableDateTimeConverter(Func<RequestTimeZone> requestTimeZone, string[] supportedFormats)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _supportedFormats = supportedFormats ?? throw new ArgumentNullException(paramName: nameof(supportedFormats));
    }

    public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        if (_supportedFormats.Length == 0)
        {
            throw new InvalidOperationException(message: "No date time formats configured");
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

    public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
    {
        if (value.HasValue is false)
        {
            return;
        }

        if (_supportedFormats.Length == 0)
        {
            throw new InvalidOperationException(message: "No date time formats configured");
        }

        string dateString = value.Value.ToString(format: _supportedFormats[0], provider: CultureInfo.InvariantCulture);

        if (!DateTime.TryParseExact(s: dateString, format: _supportedFormats[0], provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None, result: out DateTime parsedDateTime))
        {
            throw new JsonException(message: $"Failed to parse formatted date string '{dateString}' back to DateTime");
        }

        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;



        try
        {
            DateTime convertTime = TimeZoneInfo.ConvertTime(dateTime: parsedDateTime, sourceTimeZone: TimeZoneInfo.Utc,
                destinationTimeZone: timeZone);

            writer.WriteStringValue(value: convertTime.ToString(format: _supportedFormats[0],
                provider: CultureInfo.InvariantCulture));

            writer.Flush();
        }
        catch (Exception ex)
        {
            throw new JsonException(message: "Failed to convert DateTime to the specified timezone",
                innerException: ex);
        }
    }
}