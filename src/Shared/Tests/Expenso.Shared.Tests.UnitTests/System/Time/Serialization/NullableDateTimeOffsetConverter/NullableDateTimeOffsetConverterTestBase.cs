using Expenso.Shared.System.Time.Constants;
using Expenso.Shared.System.Time.Request;
using Expenso.Shared.Tests.Utils.UnitTests;

using NUnit.Framework;

namespace Expenso.Shared.Tests.UnitTests.System.Time.Serialization.NullableDateTimeOffsetConverter;

[TestFixture]
internal abstract class
    NullableDateTimeOffsetConverterTestBase : TestBase<Shared.System.Time.Serialization.NullableDateTimeOffsetConverter>
{
    [SetUp]
    public void SetUp()
    {
    }

    [TearDown]
    public void TearDown()
    {
        TestCandidate = null!;
    }

    protected void CreateTestCandidate(string timeZoneName = TimeZoneIds.Utc, string[]? supportedFormats = null)
    {
        _requestTimeZone = () => new RequestTimeZone(name: timeZoneName);

        TestCandidate = new Shared.System.Time.Serialization.NullableDateTimeOffsetConverter(
            requestTimeZone: _requestTimeZone, supportedFormats: supportedFormats ?? [DateTimeFormats.Iso8601TimeZone]);
    }

    private Func<RequestTimeZone>? _requestTimeZone;
}