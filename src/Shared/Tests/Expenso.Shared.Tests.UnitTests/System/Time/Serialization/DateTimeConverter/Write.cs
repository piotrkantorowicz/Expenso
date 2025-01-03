using System.Buffers;
using System.Text;
using System.Text.Json;

using Expenso.Shared.System.Time.Constants;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.DateTimeConverter;

[TestFixture]
internal sealed class Write : DateTimeConverterTestBase
{
    [Test, TestCaseSource(sourceName: nameof(ValidDateTimeCases))]
    public void Should_ReturnSerializedDateTime_When_ValidDateTime(DateTime dateTime, string timeZoneId,
        string expected)
    {
        // Arrange
        CreateTestCandidate(timeZoneName: timeZoneId);

        // Act
        ArrayBufferWriter<byte> bufferWriter = new();
        using Utf8JsonWriter writer = new(bufferWriter: bufferWriter);
        TestCandidate.Write(writer: writer, value: dateTime, options: new JsonSerializerOptions());
        writer.Flush();
        string result = Encoding.UTF8.GetString(bytes: bufferWriter.WrittenMemory.Span);

        // Assert
        result.Should().Be(expected: expected);
    }

    [Test]
    public void Should_ThrowTimeZoneNotFoundException_When_InvalidTimeZoneId()
    {
        // Arrange
        CreateTestCandidate(timeZoneName: "Invalid/TimeZone");

        // Act
        Action action = () =>
        {
            DateTime dateTime = DateTime.UtcNow;
            ArrayBufferWriter<byte> bufferWriter = new();
            Utf8JsonWriter writer = new(bufferWriter: bufferWriter);
            TestCandidate.Write(writer: writer, value: dateTime, options: new JsonSerializerOptions());
            writer.Dispose();
        };

        // Assert
        action.Should().Throw<TimeZoneNotFoundException>().WithMessage(expectedWildcardPattern: "*Invalid/TimeZone*");
    }
    private static IEnumerable<object> ValidDateTimeCases()
    {
        yield return new object[]
        {
            new DateTime(year: 2024, month: 3, day: 10, hour: 14, minute: 0, second: 0, kind: DateTimeKind.Utc),
            TimeZoneIds.Utc,
            "\"2024-03-10T14:00:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2024, month: 3, day: 11, hour: 5, minute: 0, second: 0, kind: DateTimeKind.Utc),
            "Pacific/Honolulu",
            "\"2024-03-10T19:00:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2023, month: 12, day: 25, hour: 8, minute: 0, second: 0, kind: DateTimeKind.Utc),
            "America/Los_Angeles",
            "\"2023-12-25T00:00:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2022, month: 1, day: 1, hour: 02, minute: 30, second: 45, kind: DateTimeKind.Utc),
            "Australia/Brisbane",
            "\"2022-01-01T12:30:45\""
        };

        yield return new object[]
        {
            new DateTime(year: 2021, month: 7, day: 4, hour: 14, minute: 45, second: 0, kind: DateTimeKind.Utc),
            "Asia/Baku",
            "\"2021-07-04T18:45:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2020, month: 2, day: 29, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc),
            "Atlantic/Reykjavik",
            "\"2020-02-29T23:59:59\""
        };

        yield return new object[]
        {
            new DateTime(year: 9999, month: 12, day: 31, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc),
            TimeZoneIds.Utc,
            "\"9999-12-31T23:59:59\""
        };

        yield return new object[]
        {
            new DateTime(year: 1, month: 1, day: 1, hour: 0, minute: 0, second: 0, kind: DateTimeKind.Utc),
            TimeZoneIds.Utc,
            "\"0001-01-01T00:00:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2024, month: 3, day: 10, hour: 2, minute: 30, second: 0, kind: DateTimeKind.Utc),
            "America/New_York",
            "\"2024-03-09T21:30:00\""
        };

        yield return new object[]
        {
            new DateTime(year: 2024, month: 11, day: 3, hour: 1, minute: 30, second: 0, kind: DateTimeKind.Utc),
            "America/New_York",
            "\"2024-11-02T21:30:00\""
        };
    }
}