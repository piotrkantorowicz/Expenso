using System.Text;
using System.Text.Json;

using FluentAssertions;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.DateTimeConverter;

[TestFixture]
internal sealed class Read : DateTimeConverterTestBase
{
    [Test]
    public void Should_ReturnParsedDateTime_When_ValidString()
    {
        const string json = "\"2024-03-10T14:00:00\"";
        Utf8JsonReader reader = new(jsonData: Encoding.UTF8.GetBytes(s: json));
        reader.Read();

        DateTime result = TestCandidate.Read(reader: ref reader, typeToConvert: typeof(DateTime),
            options: new JsonSerializerOptions());

        result
            .Should()
            .Be(expected: new DateTime(year: 2024, month: 3, day: 10, hour: 14, minute: 0, second: 0,
                kind: DateTimeKind.Utc));
    }

    [Test]
    public void Should_ReturnDefault_When_InvalidString()
    {
        const string json = "\"invalid-date\"";
        Utf8JsonReader reader = new(jsonData: Encoding.UTF8.GetBytes(s: json));
        reader.Read();

        DateTime result = TestCandidate.Read(reader: ref reader, typeToConvert: typeof(DateTime),
            options: new JsonSerializerOptions());

        result.Should().Be(expected: default);
    }
}