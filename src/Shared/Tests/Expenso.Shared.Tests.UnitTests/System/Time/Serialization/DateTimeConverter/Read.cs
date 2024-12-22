using System.Text;
using System.Text.Json;

using Expenso.Shared.System.Time.Constants;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.DateTimeConverter;

[TestFixture]
internal sealed class Read : DateTimeConverterTestBase
{
    [Test]
    [TestCaseSource(sourceName: nameof(DateTimeCases))]
    public void Should_ReturnParsedDateTime_When_ValidString(string json, string timeZoneId, DateTime expected)
    {
        // Arrange
        CreateTestCandidate(timeZoneName: timeZoneId);

        // Act
        Utf8JsonReader reader = new(jsonData: Encoding.UTF8.GetBytes(s: json));
        reader.Read();

        DateTime result = TestCandidate.Read(reader: ref reader, typeToConvert: typeof(DateTime),
            options: new JsonSerializerOptions());

        // Assert
        result.Should().Be(expected: expected);
    }

    [Test]
    [TestCase(arg: "\"invalid-date\"")]
    [TestCase(arg: "null")]
    public void Should_ThrowJsonException_When_InvalidString(string json)
    {
        // Arrange
        // Act
        CreateTestCandidate();

        Action action = () =>
        {
            Utf8JsonReader reader = new(jsonData: Encoding.UTF8.GetBytes(s: json));
            reader.Read();

            TestCandidate.Read(reader: ref reader, typeToConvert: typeof(DateTime),
                options: new JsonSerializerOptions());
        };

        // Assert
        action.Should().Throw<JsonException>();
    }

    private static IEnumerable<object> DateTimeCases()
    {
        yield return new object[]
        {
            "\"2024-03-10T14:00:00\"",
            TimeZoneIds.Utc,
            new DateTime(year: 2024, month: 3, day: 10, hour: 14, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "\"2024-03-10T19:00:00\"",
            "Pacific/Honolulu",
            new DateTime(year: 2024, month: 3, day: 11, hour: 5, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "\"2023-12-25T00:00:00\"",
            "America/Los_Angeles",
            new DateTime(year: 2023, month: 12, day: 25, hour: 8, minute: 0, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "\"2022-01-01T12:30:45\"",
            "Australia/Brisbane",
            new DateTime(year: 2022, month: 1, day: 1, hour: 02, minute: 30, second: 45, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "\"2021-07-04T18:45:00\"",
            "Asia/Baku",
            new DateTime(year: 2021, month: 7, day: 4, hour: 14, minute: 45, second: 0, kind: DateTimeKind.Utc)
        };

        yield return new object[]
        {
            "\"2020-02-29T23:59:59\"",
            "Atlantic/Reykjavik",
            new DateTime(year: 2020, month: 2, day: 29, hour: 23, minute: 59, second: 59, kind: DateTimeKind.Utc)
        };
    }
}