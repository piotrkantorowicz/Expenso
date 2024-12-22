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

        if (string.IsNullOrEmpty(value: value))
        {
            throw new JsonException(message: "DateTime string cannot be null or empty");
        }

        if (!DateTime.TryParseExact(s: value, format: _format, provider: CultureInfo.InvariantCulture,
                style: DateTimeStyles.None, result: out DateTime dateTime))
        {
            throw new JsonException(message: $"DateTime string '{value}' does not match expected format '{_format}'");
        }

        TimeZoneInfo timeZone = _requestTimeZone().TimeZone;

        return TimeZoneInfo.ConvertTime(dateTime: dateTime, destinationTimeZone: timeZone);
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
            throw new JsonException(message: $"Failed to parse formatted date string '{dateString}' back to DateTime");
        }

        TimeZoneInfo? timeZone = _requestTimeZone().TimeZone;

        if (timeZone == null)
        {
            throw new InvalidOperationException(message: "TimeZone cannot be null");
        }

        try
        {
            DateTime convertTime = TimeZoneInfo
                .ConvertTime(dateTime: parsedDateTime, destinationTimeZone: timeZone)
                .ToUniversalTime();

            writer.WriteStringValue(value: convertTime);
            writer.Flush();
        }
        catch (TimeZoneNotFoundException ex)
        {
            throw new JsonException(message: "Failed to convert DateTime to the specified timezone",
                innerException: ex);
        }
    }
}