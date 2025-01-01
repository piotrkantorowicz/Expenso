using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

using Expenso.Shared.System.Time.Request;

namespace Expenso.Shared.System.Time.Serialization;

internal sealed class NullableDateTimeOffsetConverter : JsonConverter<DateTimeOffset?>
{
    private readonly string[] _supportedFormats;
    private readonly Func<RequestTimeZone> _requestTimeZone;

    public NullableDateTimeOffsetConverter(Func<RequestTimeZone> requestTimeZone, string[] supportedFormats)
    {
        _requestTimeZone = requestTimeZone ?? throw new ArgumentNullException(paramName: nameof(requestTimeZone));
        _supportedFormats = supportedFormats ?? throw new ArgumentNullException(paramName: nameof(supportedFormats));
    }

    public override DateTimeOffset? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? value = reader.GetString();

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        if (!DateTimeOffset.TryParseExact(input: value, format: _supportedFormats[0],
                formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None,
                result: out DateTimeOffset dateTimeOffset))
        {
            throw new JsonException(
                message: $"DateTime string '{value}' does not match expected format '{_supportedFormats[0]}'");
        }

        return TimeZoneInfo.ConvertTime(dateTimeOffset: dateTimeOffset,
            destinationTimeZone: _requestTimeZone().TimeZone);
    }

    public override void Write(Utf8JsonWriter writer, DateTimeOffset? value, JsonSerializerOptions options)
    {
        if (value.HasValue is false)
        {
            return;
        }

        string dateString =
            value.Value.ToString(format: _supportedFormats[0], formatProvider: CultureInfo.InvariantCulture);

        if (!DateTimeOffset.TryParseExact(input: dateString, format: _supportedFormats[0],
                formatProvider: CultureInfo.InvariantCulture, styles: DateTimeStyles.None,
                result: out DateTimeOffset parsedDateTimeOffset))
        {
            throw new JsonException(
                message: $"Failed to parse formatted date string '{dateString}' back to DateTimeOffset");
        }

        TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;

        if (timeZone == null)
        {
            throw new InvalidOperationException(message: "TimeZone cannot be null");
        }

        try
        {
            DateTimeOffset convertTime =
                TimeZoneInfo.ConvertTime(dateTimeOffset: parsedDateTimeOffset, destinationTimeZone: timeZone);

            writer.WriteStringValue(value: convertTime.ToString(format: _supportedFormats[0],
                formatProvider: CultureInfo.InvariantCulture));

            writer.Flush();
        }
        catch (Exception ex)
        {
            throw new JsonException(message: "Failed to convert DateTimeOffset to the specified timezone",
                innerException: ex);
        }
    }
}